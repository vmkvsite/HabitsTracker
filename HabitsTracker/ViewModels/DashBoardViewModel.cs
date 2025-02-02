namespace HabitsTracker.ViewModels
{
    public class DashboardViewModel
    {
        public List<HabitViewModel> Habits { get; set; } = new();
    }

    public class HabitViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompletedToday { get; set; }
    }
}