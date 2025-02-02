public class HabitViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompletedToday { get; set; }
    public string? TargetTimeDisplay { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool ShowCalendar { get; set; }
    public List<DateTime> CompletionDates { get; set; } = new List<DateTime>();
    public DateTime ViewMonth { get; set; }  // Add this property
}