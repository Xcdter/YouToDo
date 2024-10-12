using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebTest2.Models;
using YouToDo.Repositories;
using static WebTest2.Controllers.UserController;

namespace WebTest2.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        private string _confirmPassword;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        public class LoginModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            [DataType(DataType.Password)]
            public string Confirm_Password { get; set; }
        }

        [HttpPost]
        public IActionResult Registration(User user, LoginModel model)
        {
            if (model.Password == model.Confirm_Password)
            {
                if (!_context.Users.Any(u => u.Email == user.Email))
                {
                    user.CreatedDate = DateTime.Now.ToUniversalTime();

                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    _context.Users.Add(user);

                    _context.SaveChanges();

                    return Redirect("/Home");
                }
                else
                {
                    ModelState.AddModelError("EmailIsNotFree", "Пользователь с таким Email уже существует.");
                    return View();
                }
            }
            else
            {
                return View();
            }           
        }
    }
}
