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
    public class ClassroomMaterialViewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassroomMaterialViewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClassroomMaterialViews
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClassroomMaterialViews.ToListAsync());
        }

        // GET: ClassroomMaterialViews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomMaterialView = await _context.ClassroomMaterialViews
                .FirstOrDefaultAsync(m => m.ClassroomMaterialViewId == id);
            if (classroomMaterialView == null)
            {
                return NotFound();
            }

            return View(classroomMaterialView);
        }

        // GET: ClassroomMaterialViews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassroomMaterialViews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassroomMaterialViewId,MaterialId,StudentId,ViewDate")] ClassroomMaterialView classroomMaterialView)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroomMaterialView);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classroomMaterialView);
        }

        // GET: ClassroomMaterialViews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomMaterialView = await _context.ClassroomMaterialViews.FindAsync(id);
            if (classroomMaterialView == null)
            {
                return NotFound();
            }
            return View(classroomMaterialView);
        }

        // POST: ClassroomMaterialViews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassroomMaterialViewId,MaterialId,StudentId,ViewDate")] ClassroomMaterialView classroomMaterialView)
        {
            if (id != classroomMaterialView.ClassroomMaterialViewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroomMaterialView);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomMaterialViewExists(classroomMaterialView.ClassroomMaterialViewId))
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
            return View(classroomMaterialView);
        }

        // GET: ClassroomMaterialViews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomMaterialView = await _context.ClassroomMaterialViews
                .FirstOrDefaultAsync(m => m.ClassroomMaterialViewId == id);
            if (classroomMaterialView == null)
            {
                return NotFound();
            }

            return View(classroomMaterialView);
        }

        // POST: ClassroomMaterialViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroomMaterialView = await _context.ClassroomMaterialViews.FindAsync(id);
            if (classroomMaterialView != null)
            {
                _context.ClassroomMaterialViews.Remove(classroomMaterialView);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomMaterialViewExists(int id)
        {
            return _context.ClassroomMaterialViews.Any(e => e.ClassroomMaterialViewId == id);
        }
    }
}
