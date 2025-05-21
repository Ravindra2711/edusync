using System;
using System.Collections.Generic;

namespace EduSync.Api.DTOs
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; } = new();
        // Do not include Assessment or navigation properties!
        public string CorrectAnswer { get; set; }
    }
}