using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        [ForeignKey("IdentityUser")]
        public string? UserId { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        [Required]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string? ProfilePicture { get; set; }  // file name of image

        public string? Phone { get; set; }

        // Foreign Key to Department
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Link to ASP.NET Identity User
        // Optional navigation property
        public IdentityUser? User { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
