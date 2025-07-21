namespace StudentManagementSystem_Rukaiya.Models
{
    public class AppSettings
    {
        public EmailSettings EmailSettings { get; set; } = new();
        public int AutoRedirectDelaySeconds { get; set; } = 5;
    }

    public class EmailSettings
    {
        public bool IsManualConfirmationEnabled { get; set; } = true;
    }
}