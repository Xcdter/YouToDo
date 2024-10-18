using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YouToDo.Filters;

namespace YouToDo.Controllers
{
    public class TaskController : Controller
    {
        [SessionAuthorize]
        public ActionResult Edit()
        {
            return View();
        }
    }
}
