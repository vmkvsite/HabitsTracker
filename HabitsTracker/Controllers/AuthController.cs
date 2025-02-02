using Microsoft.AspNetCore.Mvc;
using HabitsTracker.Services;
using HabitsTracker.ViewModels;

namespace HabitsTracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (HttpContext.Session.GetString("CurrentUser") != null)
            {
                return RedirectToAction("Dashboard", "HabitTracker");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var isValid = await _userService.ValidateUserAsync(model.Username, model.Password);
            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            var user = await _userService.GetUserByUsernameAsync(model.Username);
            if (user != null)
            {
                HttpContext.Session.SetString("CurrentUser", user.UserId.ToString());
                return RedirectToAction("Dashboard", "HabitTracker");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = await _userService.GetUserByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            await _userService.CreateUserAsync(model.Username, model.Password);
            return RedirectToAction(nameof(SignIn));
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(SignIn));
        }
    }
}