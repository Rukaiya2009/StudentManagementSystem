using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem_Rukaiya.Controllers
{
    [Authorize]
    public class AnnouncementCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnnouncementComments
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnnouncementComments.ToListAsync());
        }

        // GET: AnnouncementComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcementComment = await _context.AnnouncementComments
                .FirstOrDefaultAsync(m => m.AnnouncementCommentId == id);
            if (announcementComment == null)
            {
                return NotFound();
            }

            return View(announcementComment);
        }

        // GET: AnnouncementComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnnouncementComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnnouncementCommentId,AnnouncementId,UserId,CommentText,CommentDate")] AnnouncementComment announcementComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(announcementComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcementComment);
        }

        // GET: AnnouncementComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcementComment = await _context.AnnouncementComments.FindAsync(id);
            if (announcementComment == null)
            {
                return NotFound();
            }
            return View(announcementComment);
        }

        // POST: AnnouncementComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnnouncementCommentId,AnnouncementId,UserId,CommentText,CommentDate")] AnnouncementComment announcementComment)
        {
            if (id != announcementComment.AnnouncementCommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcementComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementCommentExists(announcementComment.AnnouncementCommentId))
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
            return View(announcementComment);
        }

        // GET: AnnouncementComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcementComment = await _context.AnnouncementComments
                .FirstOrDefaultAsync(m => m.AnnouncementCommentId == id);
            if (announcementComment == null)
            {
                return NotFound();
            }

            return View(announcementComment);
        }

        // POST: AnnouncementComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcementComment = await _context.AnnouncementComments.FindAsync(id);
            if (announcementComment != null)
            {
                _context.AnnouncementComments.Remove(announcementComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementCommentExists(int id)
        {
            return _context.AnnouncementComments.Any(e => e.AnnouncementCommentId == id);
        }
    }
}
