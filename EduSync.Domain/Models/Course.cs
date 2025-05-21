// File: EduSync.Domain/Models/Course.cs
namespace EduSync.Domain.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Fix: Add the missing 'Assessments' property to the Course class  
        public List<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}
