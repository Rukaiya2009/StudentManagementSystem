using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

namespace StudentManagementSystem.Services
{
    public class DevEmailSender : IEmailSender<IdentityUser>
    {

        public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
        {
            Debug.WriteLine("====== DEV EMAIL SENDER - CONFIRMATION LINK ======");
            Debug.WriteLine($"To: {email}");
            Debug.WriteLine($"Subject: Confirm your email");
            Debug.WriteLine($"Confirmation Link: {confirmationLink}");
            Debug.WriteLine("================================================");

            // Auto-open the confirmation link
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start \"{confirmationLink}\"") { CreateNoWindow = true });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠️ Failed to open confirmation link automatically: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
        {
            Debug.WriteLine("====== DEV EMAIL SENDER - PASSWORD RESET LINK ======");
            Debug.WriteLine($"To: {email}");
            Debug.WriteLine($"Subject: Reset your password");
            Debug.WriteLine($"Reset Link: {resetLink}");
            Debug.WriteLine("==================================================");

            // Auto-open the reset link
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start \"{resetLink}\"") { CreateNoWindow = true });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠️ Failed to open reset link automatically: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
        {
            Debug.WriteLine("====== DEV EMAIL SENDER - PASSWORD RESET CODE ======");
            Debug.WriteLine($"To: {email}");
            Debug.WriteLine($"Subject: Reset your password");
            Debug.WriteLine($"Reset Code: {resetCode}");
            Debug.WriteLine("==================================================");

            return Task.CompletedTask;
        }
    }
}