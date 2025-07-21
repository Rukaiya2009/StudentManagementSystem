using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class ClassroomAnnouncement
    {
        public int ClassroomAnnouncementId { get; set; } // Primary Key
        public int ClassroomId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime AnnouncementDate { get; set; }
        public Classroom? Classroom { get; set; }
    }
}