using System;
using System.Collections.Generic;

namespace EduSync.Domain.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; } = new();
        public string CorrectAnswer { get; set; }

        // Foreign key to Assessment
        public Guid AssessmentId { get; set; }
        public Assessment Assessment { get; set; }
    }
}
