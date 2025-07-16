using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public UserProfilesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: /UserProfiles/Edit
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound();

            var profile = await _context.UserProfiles.FindAsync(userId) ?? new UserProfile { UserId = userId };
            return View(profile);
        }

        // POST: /UserProfiles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfile profile, string CroppedImageData)
        {
            if (!ModelState.IsValid) return View(profile);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound();

            profile.UserId = userId;

            if (!string.IsNullOrEmpty(CroppedImageData))
            {
                var imageName = $"{Guid.NewGuid()}.png";
                var imagePath = Path.Combine(_env.WebRootPath, "images/uploads", imageName);
                var base64 = CroppedImageData.Split(',')[1];
                var bytes = Convert.FromBase64String(base64);
                await System.IO.File.WriteAllBytesAsync(imagePath, bytes);
                profile.ProfilePicture = $"/images/uploads/{imageName}";
            }

            var existing = await _context.UserProfiles.FindAsync(userId);
            if (existing == null)
                _context.Add(profile);
            else
            {
                existing.FullName = profile.FullName;
                existing.ProfilePicture = profile.ProfilePicture ?? existing.ProfilePicture;
                _context.Update(existing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }
    }
} 