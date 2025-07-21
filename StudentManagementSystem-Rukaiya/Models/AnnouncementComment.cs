using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class AnnouncementComment
    {
        public int AnnouncementCommentId { get; set; } // Primary Key
        public int AnnouncementId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CommentText { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; } = DateTime.Now;
    }
}