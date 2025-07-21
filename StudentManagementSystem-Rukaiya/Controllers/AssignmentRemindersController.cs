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
    // //[Authorize]
    public class AssignmentRemindersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRemindersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssignmentReminders
        public async Task<IActionResult> Index()
        {
            return View(await _context.AssignmentReminders.ToListAsync());
        }

        // GET: AssignmentReminders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentReminder = await _context.AssignmentReminders
                .FirstOrDefaultAsync(m => m.AssignmentReminderId == id);
            if (assignmentReminder == null)
            {
                return NotFound();
            }

            return View(assignmentReminder);
        }

        // GET: AssignmentReminders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssignmentReminders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignmentReminderId,AssignmentId,ReminderDate,IsSent")] AssignmentReminder assignmentReminder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignmentReminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assignmentReminder);
        }

        // GET: AssignmentReminders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentReminder = await _context.AssignmentReminders.FindAsync(id);
            if (assignmentReminder == null)
            {
                return NotFound();
            }
            return View(assignmentReminder);
        }

        // POST: AssignmentReminders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignmentReminderId,AssignmentId,ReminderDate,IsSent")] AssignmentReminder assignmentReminder)
        {
            if (id != assignmentReminder.AssignmentReminderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentReminder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentReminderExists(assignmentReminder.AssignmentReminderId))
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
            return View(assignmentReminder);
        }

        // GET: AssignmentReminders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentReminder = await _context.AssignmentReminders
                .FirstOrDefaultAsync(m => m.AssignmentReminderId == id);
            if (assignmentReminder == null)
            {
                return NotFound();
            }

            return View(assignmentReminder);
        }

        // POST: AssignmentReminders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignmentReminder = await _context.AssignmentReminders.FindAsync(id);
            if (assignmentReminder != null)
            {
                _context.AssignmentReminders.Remove(assignmentReminder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentReminderExists(int id)
        {
            return _context.AssignmentReminders.Any(e => e.AssignmentReminderId == id);
        }
    }
}
