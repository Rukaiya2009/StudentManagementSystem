using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class UserProfile
    {
        [Key]
        public required string UserId { get; set; }

        [Display(Name = "Full Name")]
        public required string FullName { get; set; }

        public string ProfilePicture { get; set; } = "/images/default-avatar.png";

        // Optional fields
        public string? Role { get; set; }
    }
} 