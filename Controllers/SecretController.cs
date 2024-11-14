using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YouToDo.Filters;

namespace YouToDo.Controllers
{
    [SessionAuthorize]
    public class SecretController : Controller
    {
        public IActionResult Wow()
        {
            return View();
        }
    }
}
