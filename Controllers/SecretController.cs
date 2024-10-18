using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YouToDo.Filters;

namespace YouToDo.Controllers
{
    public class SecretController : Controller
    {
        [SessionAuthorize]
        public IActionResult Wow()
        {
            return View();
        }
    }
}
