using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        // Optional: Link to Identity User
        [ForeignKey("IdentityUser")]
        public string? UserId { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; } = Gender.Male;

        public string Phone { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? ProfilePicture { get; set; }  // file name of image

        public bool IsActive { get; set; } = true;
    }
}
