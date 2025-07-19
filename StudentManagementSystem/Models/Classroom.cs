using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        [Required]
        public required string ClassroomName { get; set; }
        public string? ClassroomLink { get; set; }
        public string? Description { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
} 