using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaymentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string search, bool? isConfirmed, DateTime? fromDate, DateTime? toDate, string sortBy)
        {
            var query = _context.Payments
                .Include(p => p.Student)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Student != null &&
                    (p.Student.FullName.Contains(search) ||
                     p.Student.Email.Contains(search)));
            }

            if (isConfirmed.HasValue)
            {
                query = query.Where(p => p.IsConfirmed == isConfirmed.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.DatePaid >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.DatePaid <= toDate.Value);
            }

            query = sortBy switch
            {
                "amount_asc" => query.OrderBy(p => p.Amount),
                "amount_desc" => query.OrderByDescending(p => p.Amount),
                "date_asc" => query.OrderBy(p => p.DatePaid),
                "date_desc" => query.OrderByDescending(p => p.DatePaid),
                _ => query.OrderByDescending(p => p.DatePaid)
            };

            var payments = await query.ToListAsync();
            return View(payments);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MarkAsConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                payment.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Student: View own payments
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyPayments()
        {
            var userId = _userManager.GetUserId(User);
            if (!int.TryParse(userId, out int studentId))
            {
                return Unauthorized();
            }

            var payments = await _context.Payments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
            return View(payments);
        }

        // Student: Show upload form
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UploadProof(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (!int.TryParse(userId, out int studentId))
            {
                return Unauthorized();
            }

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == id && p.StudentId == studentId);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // Student: Handle upload POST
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProof(int id, Microsoft.AspNetCore.Http.IFormFile proofFile)
        {
            var userId = _userManager.GetUserId(User);
            if (!int.TryParse(userId, out int studentId))
            {
                return Unauthorized();
            }

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == id && p.StudentId == studentId);

            if (payment == null)
            {
                return NotFound();
            }

            if (proofFile == null || proofFile.Length == 0)
            {
                ModelState.AddModelError("proofFile", "Please select a file to upload.");
                return View(payment);
            }

            // Validate file size (max 5MB)
            if (proofFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("proofFile", "File size must be less than 5MB.");
                return View(payment);
            }

            // Validate file extension and MIME type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "application/pdf" };
            var ext = Path.GetExtension(proofFile.FileName).ToLowerInvariant();
            var mimeType = proofFile.ContentType.ToLowerInvariant();
            if (!allowedExtensions.Contains(ext) || !allowedMimeTypes.Contains(mimeType))
            {
                TempData["Error"] = "Only JPG, PNG, GIF images and PDFs are allowed.";
                return View(payment);
            }

            // Delete old file if exists
            if (!string.IsNullOrEmpty(payment.PaymentProofPath))
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath,
                    payment.PaymentProofPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            // Save file to wwwroot/uploads/paymentproofs
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "paymentproofs");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await proofFile.CopyToAsync(fileStream);
            }

            // Update payment record
            payment.PaymentProofPath = "/uploads/paymentproofs/" + uniqueFileName;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Payment proof uploaded successfully.";
            return RedirectToAction(nameof(MyPayments));
        }

        // GET: Student clicks Pay Now button - show confirmation page
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PayNow(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (!int.TryParse(userId, out int studentId))
            {
                return Unauthorized();
            }

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == id && p.StudentId == studentId && !p.IsConfirmed);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // POST: Handle simulated payment confirmation
        [Authorize(Roles = "Student")]
        [HttpPost, ActionName("PayNow")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayNowConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (!int.TryParse(userId, out int studentId))
            {
                return Unauthorized();
            }

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == id && p.StudentId == studentId && !p.IsConfirmed);

            if (payment == null)
                return NotFound();

            // Simulate payment success
            payment.IsConfirmed = true;
            payment.PaymentMethod = "Online (Simulated)";
            payment.DatePaid = DateTime.Now;
            payment.ReferenceNumber = $"SIM-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            await _context.SaveChangesAsync();

            TempData["Message"] = "Payment successfully processed (simulated).";
            return RedirectToAction(nameof(MyPayments));
        }
    }
} 