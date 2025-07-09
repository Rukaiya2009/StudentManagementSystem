using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")] // ✅ Only Admins can access this controller
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        // You can add more admin-only actions here
    }
}
