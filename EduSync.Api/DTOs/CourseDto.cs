using System;
using System.Collections.Generic;

namespace EduSync.Api.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AssessmentDto> Assessments { get; set; } = new();
    }
}