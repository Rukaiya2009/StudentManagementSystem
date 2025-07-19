using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required]
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public string? ProfilePicture { get; set; }  // file name of image

        public string? Phone { get; set; }

        // Foreign Key to Department
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Course>? Courses { get; set; }

        // Link to ASP.NET Identity User
        public string? UserId { get; set; }
        // Optional navigation property
        // public IdentityUser? User { get; set; }
    }
}
