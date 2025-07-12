using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = new();

        public int CourseId { get; set; }
        public Course Course { get; set; } = new();

        [StringLength(10)]
        public string Grade { get; set; } = string.Empty; // A, B, etc.

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
