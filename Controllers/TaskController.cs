using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YouToDo.Filters;
using YouToDo.Models;

namespace YouToDo.Controllers
{
    [SessionAuthorize]
    public class TaskController : Controller
    {

        [HttpGet]
        public ActionResult Edit()
        {
            return View(new TaskModel());
        }

        [HttpPost]
        public IActionResult SaveTask(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("List");
            }
            return View("Edit", model);
        }

        public ActionResult List()
        {
            return View();
        }
    }
}
