using Microsoft.EntityFrameworkCore;
using HabitsTracker.Models;

namespace HabitsTracker.Data
{
    public class HabitsDbContext : DbContext
    {
        public HabitsDbContext(DbContextOptions<HabitsDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<DailyHabit> Habits { get; set; }
        public DbSet<HabitCompletion> HabitCompletions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<DailyHabit>(entity =>
            {
                entity.ToTable("Habits");
                entity.HasKey(e => e.HabitId);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Habits)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HabitCompletion>(entity =>
            {
                entity.ToTable("HabitCompletions");
                entity.HasKey(e => e.CompletionId);
                entity.HasOne(e => e.Habit)
                      .WithMany(h => h.Completions)
                      .HasForeignKey(e => e.HabitId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}