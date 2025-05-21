using EduSync.Domain.Models;
using System.Security.Claims;
using EduSync.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using EduSync.Api.DTOs;

[ApiController]
[Route("api/courses/{courseId}/quizzes")]

public class QuizController : ControllerBase
{
    private readonly DataContext _context;
    public QuizController(DataContext context) => _context = context;

    [HttpGet("{quizId}")]
    [Authorize]
    public IActionResult GetQuiz(Guid courseId, Guid quizId)
    {
        var quiz = _context.Assessments
            .Include(a => a.Questions)
            .FirstOrDefault(a => a.Id == quizId && a.CourseId == courseId);

        if (quiz == null) return NotFound();

        var questions = quiz.Questions.Select(q => new QuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            Options = q.Options
            // Do not include CorrectAnswer here if you don't want to expose it
        }).ToList();

        return Ok(new
        {
            quiz.Id,
            quiz.Title,
            Questions = questions
        });
    }

    [HttpPost("{quizId}/submit")]
    [Authorize]
    public IActionResult SubmitQuiz(Guid courseId, Guid quizId, [FromBody] Dictionary<Guid, string> answers)
    {
        var quiz = _context.Assessments
            .Include(a => a.Questions)
            .FirstOrDefault(a => a.Id == quizId && a.CourseId == courseId);

        if (quiz == null) return NotFound();

        int score = 0;

        foreach (var q in quiz.Questions)
        {
            if (answers.TryGetValue(q.Id, out string submitted) && submitted == q.CorrectAnswer)
            {
                score++;
            }
        }
                   // Persist result
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = new Result
                   {
                    Id = Guid.NewGuid(),
        UserId = userId,
        CourseId = courseId,
        AssessmentId = quizId,
        Score = score,
        MaxScore = quiz.Questions.Count,
        AttemptDate = DateTime.UtcNow
           }
                ;
                _context.Results.Add(result);
                _context.SaveChanges();

        return Ok(new
        {
            Score = score,
            MaxScore = quiz.Questions.Count
        });
    }
    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public IActionResult CreateQuiz(Guid courseId, [FromBody] CreateQuizDto quizDto)
    {
        var quiz = new Assessment
        {
            Id = Guid.NewGuid(),
            Title = quizDto.Title,
            Type = quizDto.Type,
            Date = quizDto.Date,
            MaxScore = quizDto.MaxScore,
            CourseId = courseId,
            Questions = quizDto.Questions.Select(q => new Question
            {
                Id = Guid.NewGuid(),
                Text = q.Text,
                Options = q.Options,
                CorrectAnswer = q.CorrectAnswer
            }).ToList()
        };

        _context.Assessments.Add(quiz);
        _context.SaveChanges();

        // Return only the necessary data, not the full entity
        return CreatedAtAction(nameof(GetQuiz), new { courseId, quizId = quiz.Id }, new
        {
            quiz.Id,
            quiz.Title,
            quiz.Type,
            quiz.Date,
            quiz.MaxScore,
            Questions = quiz.Questions.Select(q => new
            {
                q.Id,
                q.Text,
                q.Options
            })
        });
    }

    [HttpPut("{quizId}")]
    [Authorize(Roles = "Instructor")]
    public IActionResult UpdateQuiz(Guid courseId, Guid quizId, [FromBody] CreateQuizDto quizDto)
    {
        var quiz = _context.Assessments
            .Include(a => a.Questions)
            .FirstOrDefault(a => a.Id == quizId && a.CourseId == courseId);

        if (quiz == null) return NotFound();

        quiz.Title = quizDto.Title;
        quiz.Type = quizDto.Type;
        quiz.Date = quizDto.Date;
        quiz.MaxScore = quizDto.MaxScore;

        // Remove old questions
        var oldQuestions = _context.Questions.Where(q => q.AssessmentId == quizId).ToList();
        _context.Questions.RemoveRange(oldQuestions);

        // Add new questions
        quiz.Questions = quizDto.Questions.Select(q => new Question
        {
            Id = Guid.NewGuid(),
            Text = q.Text,
            Options = q.Options,
            CorrectAnswer = q.CorrectAnswer,
            AssessmentId = quizId
        }).ToList();

        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{quizId}")]
    [Authorize(Roles = "Instructor")]
    public IActionResult DeleteQuiz(Guid courseId, Guid quizId)
    {
        var quiz = _context.Assessments
            .Include(a => a.Questions)
            .FirstOrDefault(a => a.Id == quizId && a.CourseId == courseId);

        if (quiz == null) return NotFound();

        // Remove related questions first
        if (quiz.Questions != null && quiz.Questions.Any())
        {
            _context.Questions.RemoveRange(quiz.Questions);
        }

        _context.Assessments.Remove(quiz);
        _context.SaveChanges();
        return NoContent();
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; } = new();
        // Do not include CorrectAnswer in DTO for quiz-taking endpoint
    }
}
