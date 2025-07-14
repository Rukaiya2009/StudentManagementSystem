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
            var students = await _context.Students.AsNoTracking().ToListAsync();
            
            // Debug: Log what we're loading from DB
            foreach (var s in students)
            {
                System.Diagnostics.Debug.WriteLine($"Index Load - StudentId: {s.StudentId}, Name: {s.FullName}, Gender: '{s.Gender}'");
            }
            
            return View(students);
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

            ViewBag.GenderList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<Gender>();
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
                ViewBag.GenderList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<Gender>();
                return View(existingStudentForView ?? student);
            }

            // Debug: Log received values
            System.Diagnostics.Debug.WriteLine($"Received Student Data:");
            System.Diagnostics.Debug.WriteLine($"StudentId: {student.StudentId}");
            System.Diagnostics.Debug.WriteLine($"FullName: {student.FullName}");
            System.Diagnostics.Debug.WriteLine($"Email: {student.Email}");
            System.Diagnostics.Debug.WriteLine($"Phone: {student.Phone}");
            System.Diagnostics.Debug.WriteLine($"Address: {student.Address}");
            System.Diagnostics.Debug.WriteLine($"Gender: '{student.Gender}'");
            System.Diagnostics.Debug.WriteLine($"DateOfBirth: {student.DateOfBirth}");
            
            // Also log to console for easier debugging
            Console.WriteLine($"DEBUG: Gender received: '{student.Gender}'");
            Console.WriteLine($"DEBUG: ModelState Gender: '{ModelState["Gender"]?.AttemptedValue}'");
            Console.WriteLine($"DEBUG: Request Form Gender: '{Request.Form["Gender"]}'");

            // Validate that we received the student data
            if (student == null)
            {
                TempData["ErrorMessage"] = "No student data received.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var existingStudent = await _context.Students.FindAsync(id);
                if (existingStudent == null) return NotFound();

                // 🔍 Log the incoming vs existing gender value
                System.Diagnostics.Debug.WriteLine($"Incoming Gender: '{student.Gender}'");
                System.Diagnostics.Debug.WriteLine($"Existing Gender: '{existingStudent.Gender}'");

                // Update fields directly - let EF Core handle the change detection
                existingStudent.FullName = student.FullName?.Trim() ?? existingStudent.FullName;
                existingStudent.DateOfBirth = student.DateOfBirth;
                existingStudent.Gender = student.Gender;
                existingStudent.Email = student.Email?.Trim();
                existingStudent.Phone = student.Phone?.Trim() ?? existingStudent.Phone;
                existingStudent.Address = student.Address?.Trim();

                // Debug: Log what we just did
                System.Diagnostics.Debug.WriteLine($"✅ PROPER EF UPDATE - StudentId: {existingStudent.StudentId}, Gender: '{existingStudent.Gender}'");
                Console.WriteLine($"DEBUG: After update - FullName: '{existingStudent.FullName}', Gender: '{existingStudent.Gender}'");

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

                // Save all changes using proper EF Core
                await _context.SaveChangesAsync();
                
                // Debug: Log what was actually saved
                System.Diagnostics.Debug.WriteLine($"✅ SAVED TO DB - StudentId: {existingStudent.StudentId}, Gender: '{existingStudent.Gender}'");
                Console.WriteLine($"DEBUG: Gender saved to DB: '{existingStudent.Gender}'");
                
                // Verify by reloading from database
                var verificationStudent = await _context.Students.FindAsync(id);
                Console.WriteLine($"DEBUG: Gender after reload from DB: '{verificationStudent?.Gender}'");
                
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
                // Store student in TempData for undo (serialize as JSON)
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

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
