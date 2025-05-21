using System.ComponentModel;
using EduSync.Domain.Models;

namespace EduSync.Infrastructure.Data
{
    public static class SeedInitializer
    {
        public static void Seed(DataContext context)
        {
            if (context.Courses.Any()) return; // already seeded

            var assessment1 = new Assessment
            {
                Id = Guid.NewGuid(),
                Title = "React Basics Quiz",
                Type = "Quiz",
                MaxScore = 10,
            };
            assessment1.Questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "What is JSX?",
                    Options = new List<string> { "A drink", "Syntax extension" },
                    CorrectAnswer = "Syntax extension",
                    AssessmentId = assessment1.Id // <-- Set this!
                }
            };

            var course1 = new Course
            {
                Id = Guid.NewGuid(),
                Name = "Intro to React",
                Description = "Learn the basics of React.js.",
                Assessments = new List<Assessment> { assessment1 }
            };


            var assessment2 = new Assessment
            {
                Id = Guid.NewGuid(),
                Title = "Web API Quiz",
                Type = "Quizz",
                MaxScore = 10
            };
            assessment2.Questions = new List<Question>  

            {
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Text = "What does HTTP stand for?",
                            Options = new List<string> { "HyperText Transfer Protocol", "hyper transfer text pro" },
                            CorrectAnswer = "HyperText Transfer Protocol",
                            AssessmentId = assessment2.Id // <-- Set this!
                        }
            };

            var course2 = new Course
            {
                Id = Guid.NewGuid(),
                Name = "C# Web APIs",
                Description = "Create APIs with ASP.NET Core.",
                Assessments = new List<Assessment> { assessment2 }
            };

            context.Courses.AddRange(course1, course2);
            context.SaveChanges();
        }
    }
}