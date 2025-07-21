using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
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

        [Required]
        public required int StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public required int CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public EnrollmentStatus EnrollmentStatus { get; set; } = EnrollmentStatus.Pending;
        public decimal? GPA { get; set; }
        public int? ExamMarks { get; set; }
        public Grading? Grade { get; set; }
    }
}
