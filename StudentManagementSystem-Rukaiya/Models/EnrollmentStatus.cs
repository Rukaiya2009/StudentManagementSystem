using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public enum EnrollmentStatus
    {
        [Display(Name = "Active")]
        Active,

        [Display(Name = "Completed")]
        Completed,

        [Display(Name = "Dropped")]
        Dropped,

        [Display(Name = "Pending")]
        Pending
    }
}