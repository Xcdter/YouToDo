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
using System.Text;
using YouToDo.Helpers;
using YouToDo.DTOs;

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Password != model.Confirm_Password)
            {
                ModelState.AddModelError("Confirm_Password", "Пароли не совпадают");
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("EmailIsNotFree", "Пользователь с таким Email уже существует.");
                return View(model);
            }

            user.Password = model.Password;

            user.CreatedDate = DateTime.UtcNow;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Username", user.Name);

            return RedirectToAction("Profile");
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

                    return RedirectToAction("List", "ListPage");                    
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
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // Асинхронный запрос в базу данных
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new User
            {
                Name = user.Name,
                Email = user.Email
            };

            return View(model);
        }      

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            // Получаем текущего пользователя
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Создаем ViewModel
            var model = new EditProfileDto
            {
                UserId = user.UserId,
                Name = user.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Name == model.Name)
            {
                ModelState.AddModelError(nameof(model.Name), "Вы не изменили имя.");
                return View(model);
            }

            // Изменяем имя, если оно было предоставлено
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                user.Name = model.Name;

                HttpContext.Session.SetString("Username", user.Name);
            }

            // Проверка на изменение пароля
            if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword))
            {
                if (string.IsNullOrEmpty(model.OldPassword) ||
                    !BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
                {
                    ModelState.AddModelError(nameof(model.OldPassword), ErrorMessages.WrongOldPass);
                    return View(model);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);              
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
