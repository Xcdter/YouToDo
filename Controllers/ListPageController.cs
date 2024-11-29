using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using YouToDo.Repositories;
using System.Linq;
using YouToDo.Models;
using YouToDo.Filters;
using System.Collections.Generic;

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
        public async Task<IActionResult> List(int page = 1, int pageSize = 5, int? projectId = null, short? priority = null, string tag = null)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var tasksQuery = _context.Tasks
                .Where(t => t.UserId == userId);

            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId.Value);
            }

            if (priority.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.Priority == (PriorityLevel)priority.Value);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                tasksQuery = tasksQuery.Where(t => t.Tags.Contains(tag));
            }

            tasksQuery = tasksQuery.OrderByDescending(t => t.UpdatedDate);

            var (paginatedTasks, totalPages) = await PaginateAsync(tasksQuery, page, pageSize);

            var model = new TaskProjectModel
            {
                Tasks = paginatedTasks,
                Projects = projects,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                FilteredPriority = priority.HasValue ? Enum.GetName(typeof(PriorityLevel), priority.Value) : null,
                FilteredPriorityValue = priority,
                ActiveTag = tag,
                ActiveProjectId = projectId
            };

            return View("List", model);
        }


        [HttpGet]
        public IActionResult ViewProject(int id, int page = 1, int pageSize = 1)
        {
            return RedirectToAction("List", new { projectId = id, page, pageSize });
        }


        [HttpGet]
        public IActionResult FilterByPriority(short priority, int pageSize = 1)
        {
            if (!Enum.IsDefined(typeof(PriorityLevel), priority))
            {
                return BadRequest("Invalid priority level");
            }

            return RedirectToAction("List", new { priority, pageSize });
        }

        [HttpGet]
        public IActionResult FilterByTag(string tag, int pageSize = 1)
        {
            return RedirectToAction("List", new { tag, pageSize });
        }

        private async Task<(IEnumerable<T> Items, int TotalPages)> PaginateAsync<T>(IQueryable<T> query, int page, int pageSize)
        {
            var totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedItems = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedItems, totalPages);
        }

    }
}

