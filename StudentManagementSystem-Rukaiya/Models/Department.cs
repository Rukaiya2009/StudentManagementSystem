using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        public bool IsActive { get; set; } = true;
    }
}
