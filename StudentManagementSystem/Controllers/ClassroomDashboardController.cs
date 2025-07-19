using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class ClassroomDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassroomDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.ClassroomId == id && !c.IsDeleted);

            if (classroom == null) return NotFound();

            return View(classroom);
        }
    }
} 