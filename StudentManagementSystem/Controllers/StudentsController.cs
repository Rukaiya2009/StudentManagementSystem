using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using StudentManagementSystem.Data;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.AsNoTracking().ToListAsync();
            return View(students);
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null) return NotFound();

            return View(student);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(Student student, IFormFile ProfileImage)
        {
            if (!ModelState.IsValid)
                return View(student);

            if (ProfileImage != null)
            {
                if (!IsValidImage(ProfileImage, out var validationError))
                {
                    ModelState.AddModelError("ProfileImage", validationError);
                    return View(student);
                }

                var fileName = await SaveProfileImageAsync(ProfileImage);
                student.ProfilePicture = fileName;
            }
            else
            {
                student.ProfilePicture = "/images/default-avatar.png";
            }

            _context.Add(student);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            ViewBag.GenderList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<Gender>();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int id, Student student, IFormFile ProfileImage)
        {
            if (id != student.StudentId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.GenderList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<Gender>();
                return View(student);
            }

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null) return NotFound();

            existingStudent.FullName = student.FullName?.Trim() ?? existingStudent.FullName;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Gender = student.Gender;
            existingStudent.Email = student.Email?.Trim();
            existingStudent.Phone = student.Phone?.Trim() ?? existingStudent.Phone;
            existingStudent.Address = student.Address?.Trim();

            if (ProfileImage != null)
            {
                if (!IsValidImage(ProfileImage, out var validationError))
                {
                    ModelState.AddModelError("ProfileImage", validationError);
                    ViewBag.GenderList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<Gender>();
                    return View(existingStudent);
                }

                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingStudent.ProfilePicture))
                {
                    string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingStudent.ProfilePicture.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var fileName = await SaveProfileImageAsync(ProfileImage);
                existingStudent.ProfilePicture = fileName;
            }
            else if (string.IsNullOrEmpty(existingStudent.ProfilePicture))
            {
                existingStudent.ProfilePicture = "/images/default-avatar.png";
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.StudentId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                TempData["DeletedStudent"] = System.Text.Json.JsonSerializer.Serialize(student);
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["ShowUndo"] = true;
                TempData["SuccessMessage"] = $"Student '{student.FullName}' deleted. <button class='btn btn-link p-0 m-0 align-baseline' onclick=\"undoDeleteStudent()\">Undo</button>";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UndoDelete()
        {
            if (TempData["DeletedStudent"] is string json && !string.IsNullOrEmpty(json))
            {
                var student = System.Text.Json.JsonSerializer.Deserialize<Student>(json);
                if (student != null)
                {
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Student '{student.FullName}' restored.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id) => _context.Students.Any(e => e.StudentId == id);

        // Helper method to validate image file
        private bool IsValidImage(IFormFile file, out string error)
        {
            error = "";
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
            {
                error = "Please select a valid image file (JPG, PNG, GIF)";
                return false;
            }
            if (file.Length > 5 * 1024 * 1024)
            {
                error = "File size must be less than 5MB";
                return false;
            }
            return true;
        }

        // Helper method to save image file and return relative path
        private async Task<string> SaveProfileImageAsync(IFormFile profileImage)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
            string savePath = Path.Combine(wwwRootPath, "images", fileName);

            if (!Directory.Exists(Path.Combine(wwwRootPath, "images")))
                Directory.CreateDirectory(Path.Combine(wwwRootPath, "images"));

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }
            return "/images/" + fileName;
        }
    }
}
