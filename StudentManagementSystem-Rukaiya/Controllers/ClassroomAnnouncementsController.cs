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
    ////[Authorize]
    public class ClassroomAnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassroomAnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClassroomAnnouncements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClassroomAnnouncements.Include(c => c.Classroom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClassroomAnnouncements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomAnnouncement = await _context.ClassroomAnnouncements
                .Include(c => c.Classroom)
                .FirstOrDefaultAsync(m => m.ClassroomAnnouncementId == id);
            if (classroomAnnouncement == null)
            {
                return NotFound();
            }

            return View(classroomAnnouncement);
        }

        // GET: ClassroomAnnouncements/Create
        public IActionResult Create()
        {
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName");
            return View();
        }

        // POST: ClassroomAnnouncements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassroomAnnouncementId,ClassroomId,Title,Content,AnnouncementDate")] ClassroomAnnouncement classroomAnnouncement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroomAnnouncement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", classroomAnnouncement.ClassroomId);
            return View(classroomAnnouncement);
        }

        // GET: ClassroomAnnouncements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomAnnouncement = await _context.ClassroomAnnouncements.FindAsync(id);
            if (classroomAnnouncement == null)
            {
                return NotFound();
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", classroomAnnouncement.ClassroomId);
            return View(classroomAnnouncement);
        }

        // POST: ClassroomAnnouncements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassroomAnnouncementId,ClassroomId,Title,Content,AnnouncementDate")] ClassroomAnnouncement classroomAnnouncement)
        {
            if (id != classroomAnnouncement.ClassroomAnnouncementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroomAnnouncement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomAnnouncementExists(classroomAnnouncement.ClassroomAnnouncementId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", classroomAnnouncement.ClassroomId);
            return View(classroomAnnouncement);
        }

        // GET: ClassroomAnnouncements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomAnnouncement = await _context.ClassroomAnnouncements
                .Include(c => c.Classroom)
                .FirstOrDefaultAsync(m => m.ClassroomAnnouncementId == id);
            if (classroomAnnouncement == null)
            {
                return NotFound();
            }

            return View(classroomAnnouncement);
        }

        // POST: ClassroomAnnouncements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroomAnnouncement = await _context.ClassroomAnnouncements.FindAsync(id);
            if (classroomAnnouncement != null)
            {
                _context.ClassroomAnnouncements.Remove(classroomAnnouncement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomAnnouncementExists(int id)
        {
            return _context.ClassroomAnnouncements.Any(e => e.ClassroomAnnouncementId == id);
        }
    }
}
