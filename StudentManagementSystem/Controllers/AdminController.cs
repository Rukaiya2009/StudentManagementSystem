using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Email confirmed for user: {user.Email}";
                }
                else
                {
                    TempData["Error"] = $"Failed to confirm email for user: {user.Email}";
                }
            }
            else
            {
                TempData["Message"] = $"Email already confirmed for user: {user.Email}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEmailConfirmation(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Toggle the email confirmation status
            if (user.EmailConfirmed)
            {
                // This is a workaround - we can't directly unconfirm, but we can update the database
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);
                TempData["Message"] = $"Email confirmation disabled for user: {user.Email}";
            }
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Email confirmed for user: {user.Email}";
                }
                else
                {
                    TempData["Error"] = $"Failed to confirm email for user: {user.Email}";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
