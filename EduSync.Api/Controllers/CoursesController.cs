using EduSync.Api.DTOs;
using EduSync.Domain.Models;
using EduSync.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly DataContext _context;
    public CoursesController(DataContext context) => _context = context;

    [HttpGet]
    [Authorize]
    public IActionResult GetAll() => Ok(_context.Courses.ToList());

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetById(Guid id)
    {
        var course = _context.Courses
            .Include(c => c.Assessments)
                .ThenInclude(a => a.Questions)
            .FirstOrDefault(c => c.Id == id);

        if (course == null) return NotFound();

        var dto = new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            Assessments = course.Assessments.Select(a => new AssessmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Type = a.Type,
                Date = a.Date,
                MaxScore = a.MaxScore,
                Questions = a.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Options = q.Options,
                    CorrectAnswer = q.CorrectAnswer
                }).ToList()
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public IActionResult Create([FromBody] CourseCreateDto dto)
    {
        // Optionally get the instructor's user id from the JWT
        // var instructorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            // InstructorId = instructorId // If you have this property
        };

        _context.Courses.Add(course);
        _context.SaveChanges();

        // Return a DTO or anonymous object to avoid cycles
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, new
        {
            course.Id,
            course.Name,
            course.Description,
            course.StartDate,
            course.EndDate
        });
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor")]
    public IActionResult UpdateCourse(Guid id, Course updatedCourse)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        if (course == null) return NotFound();

        course.Name = updatedCourse.Name;
        course.Description = updatedCourse.Description;
        course.StartDate = updatedCourse.StartDate;
        course.EndDate = updatedCourse.EndDate;
        // Optionally update Assessments if needed

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor")]
    public IActionResult DeleteCourse(Guid id)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        if (course == null) return NotFound();

        _context.Courses.Remove(course);
        _context.SaveChanges();
        return NoContent();
    }
}