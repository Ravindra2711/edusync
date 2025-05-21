using System;
using System.Collections.Generic;
using static QuizController;

namespace EduSync.Api.DTOs
{
    public class AssessmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int MaxScore { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}