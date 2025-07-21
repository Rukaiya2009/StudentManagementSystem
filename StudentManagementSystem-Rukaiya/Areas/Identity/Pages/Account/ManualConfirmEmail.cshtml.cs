using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StudentManagementSystem_Rukaiya.Models;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace StudentManagementSystem_Rukaiya.Areas.Identity.Pages.Account
{
    public class ManualConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManualConfirmEmailModel(UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        private readonly AppSettings _appSettings;

        [BindProperty(SupportsGet = true)]
        public string? UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Code { get; set; }

        public bool IsConfirmed { get; set; } = false;

        public string StatusMessage { get; set; } = "";

        public int RedirectDelaySeconds { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                StatusMessage = "Invalid confirmation request. Missing user ID or confirmation code.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                StatusMessage = "User not found. The confirmation link may be invalid.";
                return Page();
            }

            if (user.EmailConfirmed)
            {
                IsConfirmed = true;
                RedirectDelaySeconds = _appSettings.AutoRedirectDelaySeconds;
                StatusMessage = "Your email has already been confirmed. You can now log in.";
                ViewData["RedirectMessage"] = $"Redirecting to login page in {_appSettings.AutoRedirectDelaySeconds} seconds...";
                return Page();
            }

            StatusMessage = "Click the 'Confirm My Email' button below to manually confirm your email address.";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                StatusMessage = "Invalid confirmation request. Missing user ID or confirmation code.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                StatusMessage = "User not found. The confirmation link may be invalid.";
                return Page();
            }

            if (user.EmailConfirmed)
            {
                IsConfirmed = true;
                RedirectDelaySeconds = _appSettings.AutoRedirectDelaySeconds;
                StatusMessage = "Your email has already been confirmed. You can now log in.";
                ViewData["RedirectMessage"] = $"Redirecting to login page in {_appSettings.AutoRedirectDelaySeconds} seconds...";
                return Page();
            }

            try
            {
                // Decode the confirmation code
                string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
                
                // Confirm the email
                var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

                if (result.Succeeded)
                {
                    IsConfirmed = true;
                    RedirectDelaySeconds = _appSettings.AutoRedirectDelaySeconds;
                    StatusMessage = "Your email has been confirmed successfully! You can now log in to your account.";
                    ViewData["RedirectMessage"] = $"Redirecting to login page in {_appSettings.AutoRedirectDelaySeconds} seconds...";
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    StatusMessage = $"Error confirming your email: {errors}. The confirmation code may have expired.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error processing confirmation: {ex.Message}. Please try again or contact support.";
            }

            return Page();
        }
    }
} 