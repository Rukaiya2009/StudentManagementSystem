using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System.IO;
using System.Text.RegularExpressions;
using StudentManagementSystem.Data;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeachersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,Name,Email,DepartmentId,ProfilePicture,Phone,UserId")] Teacher teacher, string CroppedImageData)
        {
            if (ModelState.IsValid)
            {
                // Handle cropped image
                if (!string.IsNullOrEmpty(CroppedImageData))
                {
                    var base64Data = Regex.Match(CroppedImageData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    var bytes = Convert.FromBase64String(base64Data);
                    var fileName = $"{Guid.NewGuid()}.png";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads", fileName);
                    await System.IO.File.WriteAllBytesAsync(filePath, bytes);
                    teacher.ProfilePicture = "/images/uploads/" + fileName;
                }
                else if (string.IsNullOrEmpty(teacher.ProfilePicture))
                {
                    teacher.ProfilePicture = "/images/default-avatar.png";
                }
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Teacher created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,Name,Email,DepartmentId,ProfilePicture,Phone,UserId")] Teacher teacher, string CroppedImageData)
        {
            if (id != teacher.TeacherId)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var existingTeacher = await _context.Teachers.FindAsync(id);
                    if (existingTeacher == null)
                        return NotFound();
                    existingTeacher.Name = teacher.Name;
                    existingTeacher.Email = teacher.Email;
                    existingTeacher.DepartmentId = teacher.DepartmentId;
                    // Handle image update
                    if (!string.IsNullOrEmpty(CroppedImageData))
                    {
                        var base64Data = Regex.Match(CroppedImageData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                        var bytes = Convert.FromBase64String(base64Data);
                        var fileName = $"{Guid.NewGuid()}.png";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads", fileName);
                        await System.IO.File.WriteAllBytesAsync(filePath, bytes);
                        existingTeacher.ProfilePicture = "/images/uploads/" + fileName;
                    }
                    else if (string.IsNullOrEmpty(existingTeacher.ProfilePicture))
                    {
                        existingTeacher.ProfilePicture = "/images/default-avatar.png";
                    }
                    _context.Update(existingTeacher);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Teacher updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var existingTeacherForView = await _context.Teachers.FindAsync(id);
            return View(existingTeacherForView ?? teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                TempData["DeletedTeacher"] = System.Text.Json.JsonSerializer.Serialize(teacher);
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                TempData["ShowUndo"] = true;
                TempData["SuccessMessage"] = $"Teacher '{teacher.Name}' deleted. <button class='btn btn-link p-0 m-0 align-baseline' onclick=\"undoDeleteTeacher()\">Undo</button>";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UndoDelete()
        {
            if (TempData["DeletedTeacher"] is string json && !string.IsNullOrEmpty(json))
            {
                var teacher = System.Text.Json.JsonSerializer.Deserialize<Teacher>(json);
                if (teacher != null)
                {
                    _context.Teachers.Add(teacher);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Teacher '{teacher.Name}' restored.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}
