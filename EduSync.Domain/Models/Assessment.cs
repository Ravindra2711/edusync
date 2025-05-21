// File: EduSync.Domain/Models/Assessment.cs
namespace EduSync.Domain.Models
{
    public class Assessment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public Guid CourseId { get; set; }
        public int MaxScore { get; set; }

        // Navigation property
        public List<Question> Questions { get; set; } = new();
    }
}
