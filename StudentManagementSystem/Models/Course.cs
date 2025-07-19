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
        public required string CourseName { get; set; }

        public string? Title { get; set; }

        [Required]
        public required int Credit { get; set; } // Number of credits for the course

        [Required, StringLength(20)]
        public required string CourseCode { get; set; }

        public decimal? Fee { get; set; } = 0.00M;

        public CourseLevel Level { get; set; } = CourseLevel.Beginner;

        [Required]
        public required int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        // New FK for Classroom
        public int? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        [Required]
        public CourseStatus Status { get; set; } = CourseStatus.Free;

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
