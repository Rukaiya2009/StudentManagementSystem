using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public enum CourseLevel
    {
        [Display(Name = "Beginner")]
        Beginner,

        [Display(Name = "Intermediate")]
        Intermediate,

        [Display(Name = "Advanced")]
        Advanced,

        [Display(Name = "Expert")]
        Expert
    }
}