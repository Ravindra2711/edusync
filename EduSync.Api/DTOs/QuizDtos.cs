namespace EduSync.Api.DTOs
{
    public class CreateQuizDto
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int MaxScore { get; set; }
        public List<CreateQuestionDto> Questions { get; set; } = new();
    }

    public class CreateQuestionDto
    {
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public string CorrectAnswer { get; set; } = string.Empty;
    }

}
