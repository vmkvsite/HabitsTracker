using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitsTracker.Models
{
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

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public List<HabitCompletion> Completions { get; set; }
    }
}