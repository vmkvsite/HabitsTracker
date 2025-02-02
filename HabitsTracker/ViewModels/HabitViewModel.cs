namespace HabitsTracker.ViewModels
{
    public class HabitViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompletedToday { get; set; }
        public string? TargetTimeDisplay { get; set; }
    }
}