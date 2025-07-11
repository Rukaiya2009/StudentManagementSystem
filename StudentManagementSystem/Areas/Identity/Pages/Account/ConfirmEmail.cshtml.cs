using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Bind query parameters to properties explicitly for GET requests
        [BindProperty(SupportsGet = true)]
        [HiddenInput(DisplayValue = false)]
        public string? UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        [HiddenInput(DisplayValue = false)]
        public string? Code { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public bool IsConfirmed { get; set; } = false;

        public string StatusMessage { get; set; } = "";

        // Explicitly accept parameters here for extra reliability
        public async Task<IActionResult> OnGetAsync(string? userId, string? code, string? returnUrl = null)
        {
            // Assign parameters to properties for use in the Razor page
            UserId = userId;
            Code = code;
            ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                StatusMessage = "Invalid confirmation request.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                StatusMessage = "Unable to load user.";
                return Page();
            }

            string decodedCode;
            try
            {
                decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
            }
            catch
            {
                StatusMessage = "Invalid confirmation code format.";
                return Page();
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                IsConfirmed = true;
                StatusMessage = "Your email has been confirmed.";
            }
            else
            {
                IsConfirmed = false;
                StatusMessage = "Error confirming your email.";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            // Redirect back to GET with parameters preserved
            return RedirectToPage(new { UserId, Code, ReturnUrl });
        }
    }
}
