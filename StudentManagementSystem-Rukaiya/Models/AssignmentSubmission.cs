using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class AssignmentSubmission
    {
        public int AssignmentSubmissionId { get; set; } // Primary Key
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? SubmissionFilePath { get; set; }
        public string? Grade { get; set; }
        public string? Remarks { get; set; }
        public Assignment? Assignment { get; set; }
        public Student? Student { get; set; }
    }
}