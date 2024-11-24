using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouToDo.Models;
using YouToDo.Repositories;

namespace YouToDo.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult CreateProject()
        {
            var model = new Project();

            return View("Edit", model);
        }

        [HttpGet]
        public IActionResult EditProject(int? id)
        {
            Project project;

            if (id.HasValue)
            {
                project = _context.Projects.AsNoTracking().FirstOrDefault(t => t.Id == id.Value);

                if (project == null)
                {
                    return NotFound();
                }
            }
            else
            {
                project = new Project();
            }

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProject(Project model)
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
                    _context.Projects.Add(model);
                }
                else
                {
                    var existingProject = await _context.Projects.FindAsync(model.Id);

                    if (existingProject == null)
                    {
                        return NotFound();
                    }

                    existingProject.Title = model.Title;
                    existingProject.Description = model.Description;

                    existingProject.UpdatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    existingProject.DueDate = model.DueDate;

                    _context.Projects.Update(existingProject);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("List", "Task");
            }

            return View("Edit", model);
        }

        //[HttpGet]
        //public async Task<IActionResult> ViewProject(int id)
        //{
        //    var userId = int.Parse(HttpContext.Session.GetString("UserId"));

        //    var projects = await _context.Projects
        //        .Where(p => p.UserId == userId)
        //        .ToListAsync();

        //    var project = projects.FirstOrDefault(p => p.Id == id);

        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    var tasks = await _context.Tasks
        //        .Where(t => t.ProjectId == id && t.UserId == userId)
        //        .OrderByDescending(t => t.UpdatedDate)
        //        .ToListAsync();

        //    var model = (Tasks: (IEnumerable<TaskModel>)tasks, Projects: (IEnumerable<Project>)projects);

        //    return Redirect("Task/List");

        //    //return View("Task/List", model);
        //}
    }
}
