using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class ClassroomMaterialView
    {
        public int ClassroomMaterialViewId { get; set; } // Primary Key
        public int MaterialId { get; set; }
        public int StudentId { get; set; }
        public DateTime ViewDate { get; set; }
    }
}