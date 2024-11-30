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

            // Получение проектов пользователя
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            // Формирование базового запроса для задач
            var tasksQuery = _context.Tasks
                .Where(t => t.UserId == userId);

            // Фильтрация по проекту
            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId.Value);
            }

            // Фильтрация по приоритету
            if (priority.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.Priority == (PriorityLevel)priority.Value);
            }

            // Фильтрация по тегу
            if (!string.IsNullOrEmpty(tag))
            {
                tasksQuery = tasksQuery.Where(t => t.Tags.Contains(tag));
            }

            // Сортировка по дате обновления
            tasksQuery = tasksQuery.OrderByDescending(t => t.UpdatedDate);

            // Постраничное отображение
            var (paginatedTasks, totalPages) = await PaginateAsync(tasksQuery, page, pageSize);

            // Создание модели для представления
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

