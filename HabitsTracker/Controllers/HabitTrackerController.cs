using Microsoft.AspNetCore.Mvc;
using HabitsTracker.Services;
using HabitsTracker.ViewModels;

namespace HabitsTracker.Controllers
{
    public class HabitTrackerController : Controller
    {
        private readonly IHabitService _habitService;
        private readonly IUserService _userService;

        public HabitTrackerController(IHabitService habitService, IUserService userService)
        {
            _habitService = habitService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var userId = Guid.Parse(userIdString);
            var habits = await _habitService.GetUserHabitsAsync(userId);

            var viewModel = new DashboardViewModel
            {
                Habits = habits.Select(h => new HabitViewModel
                {
                    Id = h.HabitId,
                    Title = h.Title,
                    IsCompletedToday = h.Completions.Any(c => c.CompletedDate.Date == DateTime.UtcNow.Date),
                    TargetTimeDisplay = h.TargetTime.HasValue ?
                        DateTime.Today.Add(h.TargetTime.Value).ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture) :
                        null
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHabit(string Title, string? Time)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var userId = Guid.Parse(userIdString);
            TimeSpan? targetTime = null;

            if (!string.IsNullOrEmpty(Time))
            {
                // Handle HTML time input format (HH:mm)
                if (DateTime.TryParse($"2000-01-01 {Time}", out var dateTime))
                {
                    targetTime = dateTime.TimeOfDay;
                }
            }

            await _habitService.CreateHabitAsync(Title, userId, targetTime);

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHabit(Guid habitId)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var userId = Guid.Parse(userIdString);
            await _habitService.DeleteHabitAsync(habitId, userId);

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCompletion(Guid habitId)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var userId = Guid.Parse(userIdString);
            await _habitService.ToggleHabitCompletionAsync(habitId, userId);

            return RedirectToAction("Dashboard");
        }
    }
}