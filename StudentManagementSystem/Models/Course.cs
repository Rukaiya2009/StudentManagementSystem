using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
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

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
