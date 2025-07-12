using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPerformer.Data;
using TaskPerformer.Models;

namespace TaskPerformer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var userTasks = await _context.todo
                .Where(t => t.UserId == userId)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                TotalTasks = userTasks.Count,
                CompletedTasks = userTasks.Count(t => t.IsCompleted),
                PendingTasks = userTasks.Count(t => !t.IsCompleted),
                TasksDueToday = userTasks.Where(t => t.DueDate.Date == DateTime.Today).ToList()
            };

            return View(model);
        }
    }
}
