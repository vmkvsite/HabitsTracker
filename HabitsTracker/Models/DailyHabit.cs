using HabitsTracker.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class DailyHabit
{
    public DailyHabit()
    {
        Title = string.Empty;
        Completions = new List<HabitCompletion>();
    }

    [Key]
    public Guid HabitId { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; }

    public TimeSpan? TargetTime { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public AppUser? User { get; set; }

    public List<HabitCompletion> Completions { get; set; }
}