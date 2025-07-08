using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        public int Credit { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? TeacherId { get; set; }  // optional
        public Teacher Teacher { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
