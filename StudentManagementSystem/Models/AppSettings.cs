namespace StudentManagementSystem.Models
{
    public class AppSettings
    {
        public bool IsManualConfirmationEnabled { get; set; } = true;
        public int AutoRedirectDelaySeconds { get; set; } = 5;
    }
} 