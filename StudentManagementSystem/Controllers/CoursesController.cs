using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: Courses
        [AllowAnonymous] // Everyone can view course list
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Courses.Include(c => c.Department).Include(c => c.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Courses/Details/5
        [AllowAnonymous] // Everyone can view course details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "Name");
            ViewBag.CourseLevelList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<CourseLevel>();
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can create
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,Credit,DepartmentId,TeacherId,Level")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", course.DepartmentId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "Name", course.TeacherId);
            ViewBag.CourseLevelList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<CourseLevel>();
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", course.DepartmentId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "Name", course.TeacherId);
            ViewBag.CourseLevelList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<CourseLevel>();
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can edit
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,Credit,DepartmentId,TeacherId,Level")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCourse = await _context.Courses.FindAsync(id);
                    if (existingCourse == null)
                    {
                        return NotFound();
                    }

                    // Update fields manually on the tracked entity
                    existingCourse.CourseName = course.CourseName;
                    existingCourse.Credit = course.Credit;
                    existingCourse.DepartmentId = course.DepartmentId;
                    existingCourse.TeacherId = course.TeacherId;
                    existingCourse.Level = course.Level;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Course updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        TempData["ErrorMessage"] = "Course not found or has been deleted.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "The course was modified by another user. Please refresh and try again.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            // Return the existing course from database to maintain state
            var existingCourseForView = await _context.Courses.FindAsync(id);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", existingCourseForView?.DepartmentId ?? course.DepartmentId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "Name", existingCourseForView?.TeacherId ?? course.TeacherId);
            ViewBag.CourseLevelList = StudentManagementSystem.Helpers.EnumHelper.ToSelectList<CourseLevel>();
            return View(existingCourseForView ?? course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")] // Only Admin and Teacher can delete
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
