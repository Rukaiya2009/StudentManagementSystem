using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male,

        [Display(Name = "Female")]
        Female,

        [Display(Name = "Other")]
        Other
    }
}