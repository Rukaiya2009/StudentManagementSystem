using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem_Rukaiya.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int? CourseId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }


        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePaid { get; set; } = DateTime.Now;

        // ✅ This is the actual file path for uploaded proof
        public string? PaymentProofPath { get; set; }

        public string? ReferenceNumber { get; set; }

        public bool IsConfirmed { get; set; } = false;

        // ✅ Foreign key to PaymentMethod
        public int PaymentMethodId { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; } = null!;

        // 🔽 Computed helper properties (OPTIONAL)
        [NotMapped]
        public bool IsPaid => IsConfirmed;

        [NotMapped]
        public string? StudentFullName => Student?.FullName;

        [NotMapped]
        public string? ProofFile => PaymentProofPath;

        public bool IsActive { get; set; } = true;
    }
}
