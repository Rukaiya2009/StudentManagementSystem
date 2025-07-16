using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public enum Grading
    {
        [Display(Name = "A+")]
        APlus = 1,
        [Display(Name = "A")]
        A = 2,
        [Display(Name = "A-")]
        AMinus = 3,
        [Display(Name = "B+")]
        BPlus = 4,
        [Display(Name = "B")]
        B = 5,
        [Display(Name = "B-")]
        BMinus = 6,
        [Display(Name = "C+")]
        CPlus = 7,
        [Display(Name = "C")]
        C = 8,
        [Display(Name = "C-")]
        CMinus = 9,
        [Display(Name = "D")]
        D = 10,
        [Display(Name = "F")]
        F = 11
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = new();

        public int CourseId { get; set; }
        public Course Course { get; set; } = new();

        [Required]
        public Grading? Grade { get; set; }

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
