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
    public class ClassroomMaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassroomMaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClassroomMaterials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClassroomMaterials.Include(c => c.Classroom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClassroomMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomMaterial = await _context.ClassroomMaterials
                .Include(c => c.Classroom)
                .FirstOrDefaultAsync(m => m.ClassroomMaterialId == id);
            if (classroomMaterial == null)
            {
                return NotFound();
            }

            return View(classroomMaterial);
        }

        // GET: ClassroomMaterials/Create
        public IActionResult Create()
        {
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName");
            return View();
        }

        // POST: ClassroomMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassroomMaterialId,ClassroomId,Title,FilePath,UploadDate")] ClassroomMaterial classroomMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroomMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", classroomMaterial.ClassroomId);
            return View(classroomMaterial);
        }

        // GET: ClassroomMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomMaterial = await _context.ClassroomMaterials.FindAsync(id);
            if (classroomMaterial == null)
            {
                return NotFound();
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", classroomMaterial.ClassroomId);
            return View(classroomMaterial);
        }

        // POST: ClassroomMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassroomMaterialId,ClassroomId,Title,FilePath,UploadDate")] ClassroomMaterial classroomMaterial)
        {
            if (id != classroomMaterial.ClassroomMaterialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroomMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomMaterialExists(classroomMaterial.ClassroomMaterialId))
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
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "ClassroomName", classroomMaterial.ClassroomId);
            return View(classroomMaterial);
        }

        // GET: ClassroomMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomMaterial = await _context.ClassroomMaterials
                .Include(c => c.Classroom)
                .FirstOrDefaultAsync(m => m.ClassroomMaterialId == id);
            if (classroomMaterial == null)
            {
                return NotFound();
            }

            return View(classroomMaterial);
        }

        // POST: ClassroomMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroomMaterial = await _context.ClassroomMaterials.FindAsync(id);
            if (classroomMaterial != null)
            {
                _context.ClassroomMaterials.Remove(classroomMaterial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomMaterialExists(int id)
        {
            return _context.ClassroomMaterials.Any(e => e.ClassroomMaterialId == id);
        }
    }
}
