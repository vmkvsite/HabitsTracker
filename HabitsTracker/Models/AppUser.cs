using System.ComponentModel.DataAnnotations;

namespace HabitsTracker.Models
{
    public class AppUser
    {
        public AppUser()
        {
            Username = string.Empty;
            PasswordHash = string.Empty;
            Habits = new List<DailyHabit>();
        }

        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public List<DailyHabit> Habits { get; set; }
    }
}