using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StudentManagementSystem.Models;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace StudentManagementSystem.Areas.Identity.Pages.Account
{
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender<IdentityUser> _emailSender;
        private readonly AppSettings _appSettings;

        public ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender<IdentityUser> emailSender, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        public string? StatusMessage { get; set; }
        public string? ConfirmationLink { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is already confirmed
                StatusMessage = "Verification email sent. Please check your email.";
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ManualConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);

            if (_appSettings.EmailSettings.IsManualConfirmationEnabled)
            {
                StatusMessage = "<strong>✅ Email Confirmation Link Generated!</strong>";
                ConfirmationLink = callbackUrl;
            }
            else
            {
                await _emailSender.SendConfirmationLinkAsync(
                    user,
                    Input.Email,
                    callbackUrl ?? string.Empty);
                StatusMessage = "Verification email sent. Please check your email.";
            }

            return Page();
        }
    }
} 