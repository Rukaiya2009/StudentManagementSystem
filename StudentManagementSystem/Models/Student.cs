using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string UserId { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; } = Gender.Male;

        [EmailAddress]
        public string? Email { get; set; }

        public string Phone { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? ProfilePicture { get; set; }  // file name of image

        public ICollection<Enrollment>? Enrollments { get; set; } = new List<Enrollment>();
    }
}
