using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem_Rukaiya.Controllers
{
    [Authorize]
    public class AssignmentSubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentSubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssignmentSubmissions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AssignmentSubmissions.Include(a => a.Assignment).Include(a => a.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AssignmentSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions
                .Include(a => a.Assignment)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AssignmentSubmissionId == id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }

            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Create
        public IActionResult Create()
        {
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "AssignmentId", "AssignmentId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Email");
            return View();
        }

        // POST: AssignmentSubmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignmentSubmissionId,AssignmentId,StudentId,SubmissionDate,SubmissionFilePath,Grade,Remarks")] AssignmentSubmission assignmentSubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignmentSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "AssignmentId", "AssignmentId", assignmentSubmission.AssignmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Email", assignmentSubmission.StudentId);
            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions.FindAsync(id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "AssignmentId", "AssignmentId", assignmentSubmission.AssignmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Email", assignmentSubmission.StudentId);
            return View(assignmentSubmission);
        }

        // POST: AssignmentSubmissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignmentSubmissionId,AssignmentId,StudentId,SubmissionDate,SubmissionFilePath,Grade,Remarks")] AssignmentSubmission assignmentSubmission)
        {
            if (id != assignmentSubmission.AssignmentSubmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentSubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentSubmissionExists(assignmentSubmission.AssignmentSubmissionId))
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
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "AssignmentId", "AssignmentId", assignmentSubmission.AssignmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Email", assignmentSubmission.StudentId);
            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions
                .Include(a => a.Assignment)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AssignmentSubmissionId == id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }

            return View(assignmentSubmission);
        }

        // POST: AssignmentSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignmentSubmission = await _context.AssignmentSubmissions.FindAsync(id);
            if (assignmentSubmission != null)
            {
                _context.AssignmentSubmissions.Remove(assignmentSubmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentSubmissionExists(int id)
        {
            return _context.AssignmentSubmissions.Any(e => e.AssignmentSubmissionId == id);
        }
    }
}
