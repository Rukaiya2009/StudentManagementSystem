using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public string DepartmentName { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
