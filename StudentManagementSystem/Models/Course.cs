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

        [Required, StringLength(20)]
        public string CourseCode { get; set; } = string.Empty;

        public int Credit { get; set; }

        public int Credits { get; set; }

        public CourseLevel Level { get; set; } = CourseLevel.Beginner;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = new();

        public int? TeacherId { get; set; }  // optional
        public Teacher Teacher { get; set; } = new();

        [Required]
        public CourseStatus Status { get; set; } = CourseStatus.Free;

        public double Fee { get; set; } = 0.0;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
