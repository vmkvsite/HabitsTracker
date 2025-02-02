using HabitsTracker.Data;
using HabitsTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitsTracker.Services
{
    public class HabitService : IHabitService
    {
        private readonly HabitsDbContext _context;

        public HabitService(HabitsDbContext context)
        {
            _context = context;
        }

        public async Task<List<DailyHabit>> GetUserHabitsAsync(Guid userId)
        {
            return await _context.Habits
                .Include(h => h.Completions)
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        public async Task<DailyHabit> CreateHabitAsync(string title, Guid userId)
        {
            var habit = new DailyHabit
            {
                Title = title,
                UserId = userId
            };

            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();
            return habit;
        }

        public async Task DeleteHabitAsync(Guid habitId, Guid userId)
        {
            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

            if (habit != null)
            {
                _context.Habits.Remove(habit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleHabitCompletionAsync(Guid habitId, Guid userId)
        {
            var habit = await _context.Habits
                .Include(h => h.Completions)
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

            if (habit == null) return;

            var todayCompletion = habit.Completions
                .FirstOrDefault(c => c.CompletedDate.Date == DateTime.UtcNow.Date);

            if (todayCompletion != null)
            {
                _context.Remove(todayCompletion);
            }
            else
            {
                habit.Completions.Add(new HabitCompletion
                {
                    CompletedDate = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsHabitCompletedTodayAsync(Guid habitId)
        {
            return await _context.HabitCompletions
                .AnyAsync(c => c.HabitId == habitId &&
                              c.CompletedDate.Date == DateTime.UtcNow.Date);
        }
    }
}