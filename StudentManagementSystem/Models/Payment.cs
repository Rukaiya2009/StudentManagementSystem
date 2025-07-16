using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public string StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePaid { get; set; } = DateTime.Now;

        public string PaymentMethod { get; set; }

        public string ReferenceNumber { get; set; }

        public bool IsConfirmed { get; set; } = false;

        public string PaymentProofPath { get; set; }

        public string StudentFullName => Student?.FullName;
    }
} 