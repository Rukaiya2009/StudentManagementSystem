using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public string MethodName { get; set; } = string.Empty; // ✅ Fix: Prevents null warning

        public string? Description { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public bool IsActive { get; set; } = true;
    }
}
