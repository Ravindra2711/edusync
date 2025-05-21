using System;
using System.Collections.Generic;

namespace EduSync.Api.DTOs
{
    public class CourseCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // Do not include Id or InstructorId here
    }
}
