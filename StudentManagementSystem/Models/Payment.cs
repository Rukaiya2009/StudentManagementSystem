using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int? CourseId { get; set; }
        public bool IsPaid { get => IsConfirmed; set => IsConfirmed = value; }
        public DateTime PaymentDate { get => DatePaid; set => DatePaid = value; }
        public string proofFile { get => PaymentProofPath; set => PaymentProofPath = value; }

        [Required]
        public required int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public required decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePaid { get; set; } = DateTime.Now;

        [Required]
        public required string PaymentMethod { get; set; }

        public string? ReferenceNumber { get; set; }

        public bool IsConfirmed { get; set; } = false;

        public string? PaymentProofPath { get; set; }

        public string? StudentFullName => Student?.FullName;
    }
} 