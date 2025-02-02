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
                    IsCompletedToday = h.Completions.Any(c => c.CompletedDate.Date == DateTime.UtcNow.Date)
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHabit([FromBody] CreateHabitViewModel model)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);
            var habit = await _habitService.CreateHabitAsync(model.Title, userId);

            return Json(new
            {
                id = habit.HabitId,
                title = habit.Title,
                isCompletedToday = false
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHabit(Guid habitId)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);
            await _habitService.DeleteHabitAsync(habitId, userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCompletion(Guid habitId)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);
            await _habitService.ToggleHabitCompletionAsync(habitId, userId);

            var isCompleted = await _habitService.IsHabitCompletedTodayAsync(habitId);
            return Json(new { isCompleted });
        }
    }
}