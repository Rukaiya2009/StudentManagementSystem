using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int ExamId { get; set; }
        public int MarksObtained { get; set; }
        public string? Grade { get; set; }
        public string? Remarks { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public Exam? Exam { get; set; }
    }
}