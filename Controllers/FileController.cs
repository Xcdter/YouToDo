using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using YouToDo.Filters;
using YouToDo.Models;
using YouToDo.Repositories;

namespace YouToDo.Controllers
{
    [SessionAuthorize]
    public class FileController : Controller
    {
        private readonly AppDbContext _context;

        public FileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadedFile, int taskId)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null) return NotFound();

                using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);

                var fileModel = new FileModel
                {
                    Name = uploadedFile.FileName,
                    Type = Path.GetExtension(uploadedFile.FileName).TrimStart('.').ToLower(),
                    Data = memoryStream.ToArray(),
                    TaskId = taskId
                };

                _context.Files.Add(fileModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewTask", "Task", new { id = taskId });
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound($"File with ID {id} was not found.");
            }

            return File(file.Data, "application/octet-stream", file.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Ищем файл в базе данных
            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound($"File with ID {id} was not found.");
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewTask", "Task", new { id = file.TaskId });
        }
    }
}
