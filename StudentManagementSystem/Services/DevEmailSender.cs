using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StudentManagementSystem.Services
{
    public class DevEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // ✅ Log to debug output for development
            Debug.WriteLine("====== DEV EMAIL SENDER ======");
            Debug.WriteLine($"To: {email}");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine("Body:");
            Debug.WriteLine(htmlMessage);
            Debug.WriteLine("==============================");

            // ✅ Add this to auto-open the link
            try
            {
                var start = htmlMessage.IndexOf("href='") + 6;
                var end = htmlMessage.IndexOf("'", start);
                if (start >= 6 && end > start)
                {
                    var link = htmlMessage.Substring(start, end - start);

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {link}") { CreateNoWindow = true });
                    }
                }
            }
            catch
            {
                Debug.WriteLine("⚠️ Failed to open link automatically.");
            }

            return Task.CompletedTask;
        }
    }
}