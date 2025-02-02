using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitsTracker.Models
{
    public class HabitCompletion
    {
        [Key]
        public Guid CompletionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid HabitId { get; set; }

        [ForeignKey("HabitId")]
        public DailyHabit? Habit { get; set; }

        public DateTime CompletedDate { get; set; } = DateTime.UtcNow;
    }
}