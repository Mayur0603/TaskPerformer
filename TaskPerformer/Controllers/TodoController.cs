using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskPerformer.Data;
using TaskPerformer.Models;

namespace TaskPerformer.Controllers
{
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string tabfilter, string searchString)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tasks = await _context.todo
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var tasksToday = tasks.Where(t => t.DueDate.Date == DateTime.Today).ToList();
            var tasksYesterday = tasks.Where(t => t.DueDate.Date == DateTime.Today.AddDays(-1)).ToList();
            var tasksFuture = tasks.Where(t => t.DueDate.Date > DateTime.Today).ToList();

            switch (tabfilter)
            {
                case "Today":
                    tasks = tasksToday;
                    break;
                case "Yesterday":
                    tasks = tasksYesterday;
                    break;
                case "Future":
                    tasks = tasksFuture;
                    break;
                default:
                    tasks = tasksToday.Concat(tasksYesterday).Concat(tasksFuture).ToList();
                    break;
            }

            ViewData["TasksToday"] = tasksToday.Any();
            ViewData["TasksYesterday"] = tasksYesterday.Any();
            ViewData["TasksFuture"] = tasksFuture.Any();

            return View(tasks);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Todo model)
        {
            // Get logged-in user's ID from session
            var userId = HttpContext.Session.GetInt32("UserId");

            // If user not logged in, redirect
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Set required properties
            model.UserId = userId.Value;
            model.CreatedAt = DateTime.Now;

            _context.todo.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
            
        }




        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            var item = await _context.todo.FindAsync(id);
            if (item != null)
            {
                item.IsCompleted = true;
                item.CompletedDate = DateTime.Now;
                await _context.SaveChangesAsync();  
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> InComplete(int id)
        {
            var item = await _context.todo.FindAsync(id);
            if (item != null)
            {
                item.IsCompleted = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.todo.FindAsync(id);    
                if (item == null)
            {
                return NotFound();
            }
            _context.todo.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Todo/UpdateTitle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTitle([FromBody] Todo todo)
        {
            var existing = await _context.todo.FindAsync(todo.Id);
            if (existing == null)
                return NotFound();

            if (existing.DueDate < DateTime.Today || existing.IsCompleted)
                return Forbid(); // Prevent updates after due date or if completed

            existing.Title = todo.Title;
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ViewDetails([FromBody] Todo todo)
        {
            var existing = await _context.todo.FindAsync(todo.Id);
            if (existing == null) return NotFound();

            existing.Discription = todo.Discription;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Todo/UpdateTitleForm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTitle(int id, string title)
        {
            var existing = await _context.todo.FindAsync(id);
            if (existing == null) return NotFound();

            if (existing.DueDate < DateTime.Today || existing.IsCompleted)
            {
                return Forbid();
            }

            existing.Title = title;
            await _context.SaveChangesAsync();
            return Ok();
        }



    }
}
