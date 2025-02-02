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
            // Start a transaction to ensure consistency
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // First, verify the habit exists and belongs to the user
                var habit = await _context.Habits
                    .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

                if (habit == null) return;

                // Check for today's completion separately
                var todayCompletion = await _context.HabitCompletions
                    .FirstOrDefaultAsync(c => c.HabitId == habitId &&
                                            c.CompletedDate.Date == DateTime.UtcNow.Date);

                if (todayCompletion != null)
                {
                    // If completion exists, remove it
                    _context.HabitCompletions.Remove(todayCompletion);
                }
                else
                {
                    // If no completion exists, add new one
                    var completion = new HabitCompletion
                    {
                        HabitId = habitId,
                        CompletedDate = DateTime.UtcNow
                    };
                    _context.HabitCompletions.Add(completion);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> IsHabitCompletedTodayAsync(Guid habitId)
        {
            return await _context.HabitCompletions
                .AnyAsync(c => c.HabitId == habitId &&
                              c.CompletedDate.Date == DateTime.UtcNow.Date);
        }
    }
}