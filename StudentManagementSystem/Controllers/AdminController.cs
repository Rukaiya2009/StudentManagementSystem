using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using System.Threading.Tasks;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ✅ NEW: Main Dashboard with Statistics
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            ViewBag.TotalTeachers = await _context.Teachers.CountAsync();
            ViewBag.TotalCourses = await _context.Courses.CountAsync();
            ViewBag.TotalDepartments = await _context.Departments.CountAsync();
            ViewBag.TotalEnrollments = await _context.Enrollments.CountAsync();
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.PendingEmailConfirmations = await _userManager.Users.CountAsync(u => !u.EmailConfirmed);
            
            return View();
        }

        public IActionResult Index()
        {
            // Existing counts
            ViewBag.TotalStudents = _context.Students.Count();
            ViewBag.TotalTeachers = _context.Teachers.Count();
            ViewBag.TotalCourses = _context.Courses.Count();

            // Payment stats
            var payments = _context.Payments.AsQueryable();
            ViewBag.TotalPayments = payments.Count();
            ViewBag.TotalConfirmedPayments = payments.Count(p => p.IsConfirmed);
            ViewBag.TotalPendingPayments = payments.Count(p => !p.IsConfirmed);
            ViewBag.TotalAmountReceived = payments.Where(p => p.IsConfirmed).Sum(p => (decimal?)p.Amount) ?? 0;

            // Monthly income chart - last 6 months
            var now = DateTime.Now;
            var monthlyIncome = payments
                .Where(p => p.IsConfirmed && p.DatePaid >= now.AddMonths(-5).Date)
                .GroupBy(p => new { p.DatePaid.Year, p.DatePaid.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(p => p.Amount)
                })
                .ToList();

            var labels = new List<string>();
            var amounts = new List<decimal>();
            for (int i = 5; i >= 0; i--)
            {
                var date = now.AddMonths(-i);
                var income = monthlyIncome
                    .FirstOrDefault(m => m.Year == date.Year && m.Month == date.Month)?.Total ?? 0;
                labels.Add(date.ToString("MMM yyyy", CultureInfo.InvariantCulture));
                amounts.Add(income);
            }
            ViewBag.PaymentLabels = labels;
            ViewBag.PaymentAmounts = amounts;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Email confirmed for user: {user.Email}";
                }
                else
                {
                    TempData["Error"] = $"Failed to confirm email for user: {user.Email}";
                }
            }
            else
            {
                TempData["Message"] = $"Email already confirmed for user: {user.Email}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEmailConfirmation(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Toggle the email confirmation status
            if (user.EmailConfirmed)
            {
                // This is a workaround - we can't directly unconfirm, but we can update the database
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);
                TempData["Message"] = $"Email confirmation disabled for user: {user.Email}";
            }
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Email confirmed for user: {user.Email}";
                }
                else
                {
                    TempData["Error"] = $"Failed to confirm email for user: {user.Email}";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
