using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebTest2.Models;
using YouToDo.Repositories;
using System.Security.Cryptography;
using static WebTest2.Controllers.UserController;
using System.Text;

namespace WebTest2.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        private string _password;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        public class RegisterModel
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
        public IActionResult Registration(User user, RegisterModel model)
        {
            if (model.Password == model.Confirm_Password)
            {
                if (!_context.Users.Any(u => u.Email == user.Email))
                {
                    user.CreatedDate = DateTime.Now.ToUniversalTime();

                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    _context.Users.Add(user);

                    _context.SaveChanges();

                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("Username", user.Name);

                    return Redirect("Task/List");
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

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }


        public class LoginModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Email == null || model.Email == null)
            {
                return View(model);
            }

            var user = await FindByEmailAsync(model.Email);

            if (user != null)
            {
                var hashedPassword = user.Password;

                if (!BCrypt.Net.BCrypt.Verify(model.Password, hashedPassword))
                {
                    ModelState.AddModelError("WrongEmailOrPass", "Неверно указан пароль.");
                    return View();
                }
                else
                {
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("Username", user.Name);

                    return Redirect("/Task/List");
                }
            }
            else
            {
                ModelState.AddModelError("WrongEmailOrPass", "Неверно указана почта");
                return View(model);
            }
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("Login"); // Redirect to login page
        }
    }
}
