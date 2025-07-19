using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class ClassroomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassroomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classroom
        public async Task<IActionResult> Index()
        {
            var classrooms = await _context.Classrooms
                .Include(c => c.Department)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
            return View(classrooms);
        }

        // GET: Classroom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var classroom = await _context.Classrooms
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.ClassroomId == id);
            if (classroom == null) return NotFound();

            return View(classroom);
        }

        // GET: Classroom/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Classroom/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", classroom.DepartmentId);
            return View(classroom);
        }

        // GET: Classroom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null) return NotFound();

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", classroom.DepartmentId);
            return View(classroom);
        }

        // POST: Classroom/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Classroom classroom)
        {
            if (id != classroom.ClassroomId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomExists(classroom.ClassroomId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", classroom.DepartmentId);
            return View(classroom);
        }

        // GET: Classroom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var classroom = await _context.Classrooms
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.ClassroomId == id);
            if (classroom == null) return NotFound();

            return View(classroom);
        }

        // POST: Classroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom != null)
            {
                classroom.IsDeleted = true;
                _context.Update(classroom);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Classroom/Undo/5
        public async Task<IActionResult> Undo(int? id)
        {
            if (id == null) return NotFound();

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null) return NotFound();

            classroom.IsDeleted = false;
            _context.Update(classroom);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomExists(int id)
        {
            return _context.Classrooms.Any(e => e.ClassroomId == id);
        }
    }
} 