using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class GradingRule
    {
        public int GradingRuleId { get; set; } // Primary Key
        public string Grade { get; set; } = string.Empty;
        public int MinMarks { get; set; }
        public int MaxMarks { get; set; }
        public string? Remarks { get; set; }
    }
}