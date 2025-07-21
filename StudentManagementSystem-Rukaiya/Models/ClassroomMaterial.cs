using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class ClassroomMaterial
    {
        public int ClassroomMaterialId { get; set; } // Primary Key
        public int ClassroomId { get; set; }
        public string? Title { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public Classroom? Classroom { get; set; }
    }
}