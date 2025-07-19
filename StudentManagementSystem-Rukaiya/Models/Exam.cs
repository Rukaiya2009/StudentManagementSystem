using System;
using System.ComponentModel.DataAnnotations;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        [Required]
        public string ExamName { get; set; } = string.Empty;
        
        public DateTime ExamDate { get; set; }

        [Required]
        public ExamType ExamType { get; set; }

        [Range(30, 300)]
        public int DurationMinutes { get; set; }

        public int? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        public bool IsDeleted { get; set; } = false;
        public Course? Course { get; set; }

        public bool IsActive { get; set; } = true;
    }
}