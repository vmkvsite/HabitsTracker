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
            var expandedHabitId = HttpContext.Session.GetString("ExpandedHabitId");

            var viewModel = new DashboardViewModel
            {
                Habits = habits.Select(h =>
                {
                    var viewMonthKey = $"ViewMonth_{h.HabitId}";
                    var viewMonthStr = HttpContext.Session.GetString(viewMonthKey);
                    var viewMonth = viewMonthStr != null ?
                        DateTime.Parse(viewMonthStr) :
                        DateTime.Today;

                    // Add this line temporarily to debug
                    System.Diagnostics.Debug.WriteLine($"ViewMonth for habit {h.HabitId}: {viewMonth:MMMM yyyy}");

                    return new HabitViewModel
                    {
                        Id = h.HabitId,
                        Title = h.Title,
                        IsCompletedToday = h.Completions.Any(c => c.CompletedDate.Date == DateTime.UtcNow.Date),
                        TargetTimeDisplay = h.TargetTime.HasValue ?
                            DateTime.Today.Add(h.TargetTime.Value).ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture) :
                            null,
                        CreatedDate = h.CreatedDate,
                        ShowCalendar = h.HabitId.ToString() == expandedHabitId,
                        CompletionDates = h.Completions.Select(c => c.CompletedDate.Date).ToList(),
                        ViewMonth = viewMonth
                    };
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

        [HttpGet]
        public async Task<IActionResult> GetHabitCompletions(Guid habitId)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);
            var habit = await _habitService.GetHabitWithCompletionsAsync(habitId, userId);

            if (habit == null)
            {
                return NotFound();
            }

            var completions = habit.Completions.Select(c => c.CompletedDate.Date).ToList();
            return Json(new { completions });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCalendar(Guid habitId)
        {
            var userIdString = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var userId = Guid.Parse(userIdString);
            var habit = await _habitService.GetHabitWithCompletionsAsync(habitId, userId);

            if (habit == null)
            {
                return NotFound();
            }

            // Get current expanded habit ID from session
            var expandedHabitId = HttpContext.Session.GetString("ExpandedHabitId");

            if (expandedHabitId == habitId.ToString())
            {
                // If this habit is already expanded, collapse it
                HttpContext.Session.Remove("ExpandedHabitId");
            }
            else
            {
                // Expand this habit
                HttpContext.Session.SetString("ExpandedHabitId", habitId.ToString());
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeMonth(Guid habitId, int monthOffset)
        {
            var viewMonthKey = $"ViewMonth_{habitId}";
            var currentViewMonth = HttpContext.Session.GetString(viewMonthKey);
            var currentDate = currentViewMonth != null ?
                DateTime.Parse(currentViewMonth) :
                DateTime.Today;

            // Calculate new date
            var newDate = currentDate.AddMonths(monthOffset);

            // Don't allow future months
            if (newDate > DateTime.Today)
            {
                newDate = DateTime.Today;
            }

            // Store new date in session
            HttpContext.Session.SetString(viewMonthKey, newDate.ToString("yyyy-MM-dd"));

            // Keep the calendar expanded
            HttpContext.Session.SetString("ExpandedHabitId", habitId.ToString());

            return RedirectToAction("Dashboard");
        }
    }
}