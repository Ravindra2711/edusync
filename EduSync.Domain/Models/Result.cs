namespace EduSync.Domain.Models
{
    public class Result
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid AssessmentId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}