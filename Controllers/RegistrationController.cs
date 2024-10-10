using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest2.Models;


namespace WebTest2.Controllers
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        [HttpPost]
        public IActionResult Save(User model)
        {


            return View("Index");
        }
    }
}
