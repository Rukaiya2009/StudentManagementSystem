using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public int EnrollmentId { get; set; }
        [ForeignKey("EnrollmentId")]
        public Enrollment Enrollment { get; set; } = new();

        [Required]
        public double Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string? PaymentMethod { get; set; }  // e.g., Card, Bank, Cash

        [Required]
        [StringLength(10)]
        public string Status { get; set; } = "Unpaid"; // Paid or Unpaid
    }
} 