using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfileImagePath { get; set; }
    }
}
