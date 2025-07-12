using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

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

            // ✅ Add this to auto-open the link and prevent token corruption
            try
            {
                var start = htmlMessage.IndexOf("href='") + 6;
                var end = htmlMessage.IndexOf("'", start);
                if (start >= 6 && end > start)
                {
                    var link = htmlMessage.Substring(start, end - start);
                    
                    // Log the clean link for debugging
                    Debug.WriteLine($"🔗 Clean confirmation link: {link}");

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Use the raw link without HTML encoding
                        Process.Start(new ProcessStartInfo("cmd", $"/c start \"{link}\"") { CreateNoWindow = true });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠️ Failed to open link automatically: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}