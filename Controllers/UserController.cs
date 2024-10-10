using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest2.Models;

namespace WebTest2.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Registration()
        {
            return View("Registration");
        }
    }
}
