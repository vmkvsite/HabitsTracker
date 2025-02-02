using System.ComponentModel.DataAnnotations;

namespace HabitsTracker.ViewModels
{
    public class CreateHabitViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Time { get; set; }
    }
}