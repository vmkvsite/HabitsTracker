using HabitsTracker.Models;

namespace HabitsTracker.Services
{
    public interface IHabitService
    {
        Task<List<DailyHabit>> GetUserHabitsAsync(Guid userId);
        Task<DailyHabit> CreateHabitAsync(string title, Guid userId, TimeSpan? targetTime = null);
        Task DeleteHabitAsync(Guid habitId, Guid userId);
        Task ToggleHabitCompletionAsync(Guid habitId, Guid userId);
        Task<bool> IsHabitCompletedTodayAsync(Guid habitId);
    }
}