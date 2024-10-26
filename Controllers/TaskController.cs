using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTest2.Models;
using YouToDo.Filters;
using YouToDo.Models;
using YouToDo.Repositories;

namespace YouToDo.Controllers
{
    [SessionAuthorize]
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));

                model.UserId = userId;

                model.CreatedDate = DateTime.Now.ToUniversalTime();

                _context.Tasks.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("List");
            }
            
            return View("Edit", model);
        }

        public async Task<IActionResult> List()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId).ToListAsync();

            return View(tasks);
        }
    }
}
