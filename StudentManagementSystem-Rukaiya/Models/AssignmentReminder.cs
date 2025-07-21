using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class AssignmentReminder
    {
        public int AssignmentReminderId { get; set; } // Primary Key
        public int AssignmentId { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsSent { get; set; } = false;
    }
}