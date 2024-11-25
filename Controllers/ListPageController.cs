using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using YouToDo.Repositories;
using System.Linq;
using YouToDo.Models;
using YouToDo.Filters;

namespace YouToDo.Controllers
{
    [SessionAuthorize]
    public class ListPageController : Controller
    {
        private readonly AppDbContext _context;

        public ListPageController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> List(int page = 1, int pageSize = 5)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.UpdatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.ActiveProjectId = null;

            var model = new TaskProjectModel
            {
                Tasks = tasks,
                Projects = projects,
                FilteredPriority = null
            };

            return View("List", model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewProject(int id)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == id && t.UserId == userId)
                .OrderByDescending(t => t.UpdatedDate)
                .ToListAsync();

            ViewBag.ActiveProjectId = id;

            var model = new TaskProjectModel
            {
                Tasks = tasks,
                Projects = projects
            };

            return View("List", model);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByPriority(short priority)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            // Преобразуем числовое значение в перечисление PriorityLevel
            if (!Enum.IsDefined(typeof(PriorityLevel), priority))
            {
                return BadRequest("Invalid priority level");
            }

            var priorityLevel = (PriorityLevel)priority;

            // Фильтрация задач по приоритету
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId && t.Priority == priorityLevel)
                .OrderByDescending(t => t.UpdatedDate)
                .ToListAsync();

            // Получение всех проектов для бокового меню
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var model = new TaskProjectModel
            {
                Tasks = tasks,
                Projects = projects,
                FilteredPriority = priorityLevel.ToString()
            };

            return View("List", model);
        }
    }
}

