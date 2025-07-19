#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using StudentManagementSystem.Data;
using System.Collections.Generic;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [TempData]
        public string? ConfirmationMessage { get; set; } // ✅ Made nullable to avoid warning

        public EnrollmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet(string? returnUrl = null, bool confirmed = false)
        {
            if (confirmed)
            {
                ConfirmationMessage = "Your email has been confirmed! You can now log in.";
            }
            return View();
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(EnrollmentStatus? statusFilter, int? courseId, int? studentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);

            IQueryable<Enrollment> enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student);

            if (roles.Contains("Student"))
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == user.Email);
                if (student == null)
                {
                    return Unauthorized();
                }

                enrollments = enrollments.Where(e => e.StudentId == student.StudentId);
            }

            // Filtering by status
            if (statusFilter.HasValue)
            {
                enrollments = enrollments.Where(e => e.EnrollmentStatus == statusFilter.Value);
            }
            // Filtering by course
            if (courseId.HasValue)
            {
                enrollments = enrollments.Where(e => e.CourseId == courseId.Value);
            }
            // Filtering by student
            if (studentId.HasValue)
            {
                enrollments = enrollments.Where(e => e.StudentId == studentId.Value);
            }

            ViewBag.StatusList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                StudentManagementSystem.Helpers.EnumHelper.ToSelectList<EnrollmentStatus>(), "Value", "Text", statusFilter?.ToString());
            ViewBag.CurrentStatus = statusFilter;
            ViewBag.Courses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName", courseId);
            ViewBag.Students = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Students.ToListAsync(), "StudentId", "FullName", studentId);

            return View(await enrollments.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);

            if (enrollment == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Check that Student and Email are not null before access
            if (roles.Contains("Student") && (enrollment.Student?.Email != user.Email))
            {
                return Forbid(); // Prevent students from viewing others' records
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([Bind("EnrollmentId,StudentId,CourseId,Grade,EnrollmentStatus")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                // Auto-create payment for paid courses only
                var course = await _context.Courses.FindAsync(enrollment.CourseId);
                if (course != null && course.Status == CourseStatus.Paid)
                {
                    var payment = new Payment
                    {
                        StudentId = enrollment.StudentId,
                        Amount = course.Fee ?? 0m,
                        DatePaid = DateTime.Now,
                        PaymentMethod = "Pending",
                        ReferenceNumber = string.Empty,
                        IsConfirmed = false,
                        PaymentProofPath = string.Empty
                    };
                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();
                }
                TempData["SuccessMessage"] = "Enrollment created successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(enrollment);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            PopulateDropdowns(enrollment);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,StudentId,CourseId,Grade,EnrollmentStatus")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEnrollment = await _context.Enrollments.FindAsync(id);
                    if (existingEnrollment == null)
                    {
                        TempData["ErrorMessage"] = "Enrollment not found or has been deleted.";
                        return RedirectToAction(nameof(Index));
                    }
                    existingEnrollment.StudentId = enrollment.StudentId;
                    existingEnrollment.CourseId = enrollment.CourseId;
                    existingEnrollment.Grade = enrollment.Grade;
                    existingEnrollment.EnrollmentStatus = enrollment.EnrollmentStatus;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Enrollment updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentId))
                    {
                        TempData["ErrorMessage"] = "Enrollment not found or has been deleted.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "The enrollment was modified by another user. Please refresh and try again.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            PopulateDropdowns(enrollment);
            var existingEnrollmentForView = await _context.Enrollments.FindAsync(id);
            return View(existingEnrollmentForView ?? enrollment);
        }

        // GET: Enrollments/Delete/5
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                TempData["DeletedEnrollment"] = System.Text.Json.JsonSerializer.Serialize(enrollment);
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                TempData["ShowUndo"] = true;
                TempData["SuccessMessage"] = $"Enrollment deleted. <button class='btn btn-link p-0 m-0 align-baseline' onclick=\"undoDeleteEnrollment()\">Undo</button>";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UndoDelete()
        {
            if (TempData["DeletedEnrollment"] is string json && !string.IsNullOrEmpty(json))
            {
                var enrollment = System.Text.Json.JsonSerializer.Deserialize<Enrollment>(json);
                if (enrollment != null)
                {
                    _context.Enrollments.Add(enrollment);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Enrollment restored.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }

        // DRY helper for form dropdowns
        private void PopulateDropdowns(Enrollment? enrollment = null)
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", enrollment?.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", enrollment?.StudentId);
            ViewBag.StatusList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<EnrollmentStatus>();
            ViewBag.GradeList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<Grading>();
        }
    }
}
#nullable disable
