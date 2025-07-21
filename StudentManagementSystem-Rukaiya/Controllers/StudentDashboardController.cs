using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_Rukaiya.Data;
using StudentManagementSystem_Rukaiya.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace StudentManagementSystem_Rukaiya.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == user.Email);

            if (student == null)
                return NotFound();

            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == student.StudentId)
                .ToListAsync();

            decimal gpa = GPAHelper.CalculateGPA(enrollments);
            ViewBag.GPA = gpa;

            ViewBag.Student = student;
            return View(enrollments);
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == user.Email);

            if (student == null)
                return NotFound();

            return View(student);
        }
    }
} 