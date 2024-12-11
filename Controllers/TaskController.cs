using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> ViewTask(int? id)
        {
            if (id == null)
            {
                return BadRequest("Task ID is required.");
            }

            var task = await _context.Tasks
                .AsNoTracking()
                .Include(t => t.Files)
                .FirstOrDefaultAsync(t => t.Id == id.Value);

            if (task == null)
            {
                return NotFound($"Task with ID {id} was not found.");
            }

            return View(task);
        }

        [HttpGet]
        public IActionResult CreateTask()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var projects = _context.Projects
                .Where(p => p.UserId == userId)
                .Select(p => new { p.Id, p.Title })
                .ToList();

            ViewBag.Projects = projects;

            return View("Edit", new TaskModel()); // Передаем пустую модель в представление
        }

        // GET: Task/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            TaskModel task;

            if (id.HasValue)
            {
                // Если `id` передан, пытаемся найти существующую задачу
                task = _context.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id.Value);

                // Если задача не найдена, возвращаем 404
                if (task == null)
                {
                    return NotFound();
                }
            }
            else
            {
                // Если `id` не передан, создаем новую пустую задачу
                task = new TaskModel();
            }

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var projects = _context.Projects
                .Where(p => p.UserId == userId)
                .Select(p => new { p.Id, p.Title })
                .ToList();

            ViewBag.Projects = projects;

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                model.UserId = userId;

                // Установка даты создания
                if (model.Id == 0)
                {
                    model.CreatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                }

                // Обработка даты завершения
                if (model.DueDate.HasValue)
                {
                    model.DueDate = DateTime.SpecifyKind(model.DueDate.Value, DateTimeKind.Utc);
                }

                if (model.Id == 0)
                {
                    _context.Tasks.Add(model); // Добавляем новую задачу
                }
                else
                {
                    var existingTask = await _context.Tasks.FindAsync(model.Id);
                    if (existingTask == null)
                    {
                        return NotFound(); // Если задача не найдена
                    }

                    // Обновляем свойства существующей задачи
                    existingTask.Title = model.Title;
                    existingTask.Description = model.Description;
                    existingTask.Priority = model.Priority;
                    existingTask.Tags = model.Tags;
                    existingTask.ProjectId = model.ProjectId;

                    existingTask.UpdatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    existingTask.DueDate = model.DueDate;

                    _context.Tasks.Update(existingTask); // Обновляем существующую задачу
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("List", "ListPage");
            }

            return View("Edit", model); // Если модель не валидна, вернуть её в представление
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(); // Если задача не найдена
            }

            _context.Tasks.Remove(task); // Удаляем задачу
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных

            return RedirectToAction("List", "ListPage"); // Перенаправляем на список задач
        }

    }
}
