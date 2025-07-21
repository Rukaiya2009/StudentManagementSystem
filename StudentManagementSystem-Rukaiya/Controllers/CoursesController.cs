using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_Rukaiya.Data;
using StudentManagementSystem_Rukaiya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem_Rukaiya.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses (Anyone can view)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var courses = _context.Courses
                .Include(c => c.Classroom)
                .Include(c => c.Department)
                .Include(c => c.Teacher);
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5 (Anyone can view)
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .Include(c => c.Classroom)
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        private void PopulateDropdowns(Course? course = null)
        {
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", course?.DepartmentId);
            ViewBag.TeacherId = new SelectList(_context.Teachers, "TeacherId", "Email", course?.TeacherId);
            ViewBag.ClassroomId = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", course?.ClassroomId);

            ViewBag.Level = Enum.GetValues(typeof(CourseLevel))
                .Cast<CourseLevel>()
                .Select(l => new SelectListItem
                {
                    Value = ((int)l).ToString(),
                    Text = l.ToString(),
                    Selected = course != null && (int)course.Level == (int)l
                });

            ViewBag.Status = Enum.GetValues(typeof(CourseStatus))
                .Cast<CourseStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = course != null && (int)course.Status == (int)s
                });
        }

        // GET: Courses/Create
        //[Authorize]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,Title,Credit,CourseCode,Fee,Level,DepartmentId,TeacherId,ClassroomId,Status,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(course);
            return View(course);
        }

        // GET: Courses/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            PopulateDropdowns(course);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,Title,Credit,CourseCode,Fee,Level,DepartmentId,TeacherId,ClassroomId,Status")] Course course)
        {
            if (id != course.CourseId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(course);
            return View(course);
        }

        // GET: Courses/Delete/5
        //[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .Include(c => c.Classroom)
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
