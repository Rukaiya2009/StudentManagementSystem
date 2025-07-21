using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public int ClassroomId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Classroom? Classroom { get; set; }
    }
}