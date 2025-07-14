using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public enum Grading
    {
        APlus, A, AMinus,
        BPlus, B, BMinus,
        CPlus, C, CMinus,
        DPlus, D, DMinus,
        F
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = new();

        public int CourseId { get; set; }
        public Course Course { get; set; } = new();

        [Required]
        public Grading? Grade { get; set; } = null;

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
