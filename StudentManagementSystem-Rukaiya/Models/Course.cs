using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public enum CourseStatus
    {
        Free,
        Paid
    }

    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; } = string.Empty;

        public string? Title { get; set; }

        [Required]
        public int Credit { get; set; } // Number of credits for the course

        [Required, StringLength(20)]
        public string CourseCode { get; set; } = string.Empty;

        public decimal? Fee { get; set; } = 0.00M;

        public CourseLevel Level { get; set; } = CourseLevel.Beginner;

        [Required]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        // New FK for Classroom
        public int? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        [Required]
        public CourseStatus Status { get; set; } = CourseStatus.Free;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public bool IsActive { get; set; } = true;
    }
}
