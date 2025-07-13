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

        // GET: Students
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(Student student, IFormFile ProfileImage)
        {
            if (ModelState.IsValid)
            {
                            if (ProfileImage != null)
            {
                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(ProfileImage.ContentType.ToLower()))
                {
                    ModelState.AddModelError("ProfileImage", "Please select a valid image file (JPG, PNG, GIF)");
                    return View(student);
                }

                // Validate file size (5MB max)
                if (ProfileImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfileImage", "File size must be less than 5MB");
                    return View(student);
                }

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                string path = Path.Combine(wwwRootPath + "/images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                student.ProfilePicture = "/images/" + fileName;
            }

                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int id, Student student, IFormFile ProfileImage)
        {
            if (id != student.StudentId) return NotFound();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"ModelState Error: {error}");
                }
                
                // Return the existing student from database to maintain state
                var existingStudentForView = await _context.Students.FindAsync(id);
                return View(existingStudentForView ?? student);
            }

            try
            {
                var existingStudent = await _context.Students.FindAsync(id);
                if (existingStudent == null) return NotFound();

                // Update fields manually on the tracked entity
                existingStudent.FullName = student.FullName;
                existingStudent.DateOfBirth = student.DateOfBirth;
                existingStudent.Gender = student.Gender;
                existingStudent.Email = student.Email;
                existingStudent.Phone = student.Phone;
                existingStudent.Address = student.Address;

                // Handle image upload
                if (ProfileImage != null)
                {
                    // Validate file type
                    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                    if (!allowedTypes.Contains(ProfileImage.ContentType.ToLower()))
                    {
                        ModelState.AddModelError("ProfileImage", "Please select a valid image file (JPG, PNG, GIF)");
                        var existingStudentForView = await _context.Students.FindAsync(id);
                        return View(existingStudentForView ?? student);
                    }

                    // Validate file size (5MB max)
                    if (ProfileImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ProfileImage", "File size must be less than 5MB");
                        var existingStudentForView = await _context.Students.FindAsync(id);
                        return View(existingStudentForView ?? student);
                    }

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingStudent.ProfilePicture))
                    {
                        string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingStudent.ProfilePicture.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Save new image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    existingStudent.ProfilePicture = "/images/" + fileName;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.StudentId)) 
                {
                    TempData["ErrorMessage"] = "Student not found or has been deleted.";
                    return RedirectToAction(nameof(Index));
                }
                else 
                {
                    TempData["ErrorMessage"] = "The student was modified by another user. Please refresh and try again.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                // Delete image file from wwwroot if exists
                if (!string.IsNullOrEmpty(student.ProfilePicture))
                {
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath, student.ProfilePicture.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
