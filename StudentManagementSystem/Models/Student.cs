using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string Phone { get; set; }

        public string? Address { get; set; }

        public string? ProfilePicture { get; set; }  // file name of image

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
