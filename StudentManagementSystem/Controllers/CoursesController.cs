using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using StudentManagementSystem.Data;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .AsNoTracking()
                .ToListAsync();

            return View(courses);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            return course is null ? NotFound() : View(course);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            PopulateViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(course.DepartmentId, course.TeacherId);
                return View(course);
            }

            _context.Add(course);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course is null) return NotFound();

            PopulateViewBags(course.DepartmentId, course.TeacherId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.CourseId) return NotFound();

            if (!ModelState.IsValid)
            {
                // Restore dropdowns and fetch the latest course for view context
                var existingCourseForView = await _context.Courses.FindAsync(id);
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", existingCourseForView?.DepartmentId ?? course.DepartmentId);
                ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "Name", existingCourseForView?.TeacherId ?? course.TeacherId);
                ViewBag.CourseLevelList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<CourseLevel>();
                return View(existingCourseForView ?? course);
            }

            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse is null) return NotFound();

            existingCourse.CourseName = course.CourseName;
            existingCourse.Credit = course.Credit;
            existingCourse.DepartmentId = course.DepartmentId;
            existingCourse.TeacherId = course.TeacherId;
            existingCourse.Level = course.Level;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.CourseId))
                {
                    TempData["ErrorMessage"] = "Course not found or has been deleted.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "The course was modified by another user. Please refresh and try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            return course is null ? NotFound() : View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                TempData["DeletedCourse"] = JsonSerializer.Serialize(course);
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                TempData["ShowUndo"] = true;
                TempData["SuccessMessage"] =
                    $"Course '{course.CourseName}' deleted. <button class='btn btn-link p-0 m-0 align-baseline' onclick=\"undoDeleteCourse()\">Undo</button>";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UndoDelete()
        {
            if (TempData["DeletedCourse"] is string json && !string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    var course = JsonSerializer.Deserialize<Course>(json);
                    if (course != null)
                    {
                        _context.Courses.Add(course);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = $"Course '{course.CourseName}' restored.";
                    }
                }
                catch (JsonException)
                {
                    TempData["ErrorMessage"] = "Failed to restore course: corrupted data.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id) =>
            _context.Courses.Any(e => e.CourseId == id);

        private void PopulateViewBags(int? departmentId = null, int? teacherId = null)
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", departmentId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "Name", teacherId);
            ViewBag.CourseLevelList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<CourseLevel>();
        }
    }
}
