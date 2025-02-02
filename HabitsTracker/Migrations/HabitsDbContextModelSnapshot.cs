﻿// <auto-generated />
using System;
using HabitsTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HabitsTracker.Migrations
{
    [DbContext(typeof(HabitsDbContext))]
    partial class HabitsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HabitsTracker.Models.AppUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("HabitsTracker.Models.DailyHabit", b =>
                {
                    b.Property<Guid>("HabitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HabitId");

                    b.HasIndex("UserId");

                    b.ToTable("Habits", (string)null);
                });

            modelBuilder.Entity("HabitsTracker.Models.HabitCompletion", b =>
                {
                    b.Property<Guid>("CompletionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HabitId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CompletionId");

                    b.HasIndex("HabitId");

                    b.ToTable("HabitCompletions", (string)null);
                });

            modelBuilder.Entity("HabitsTracker.Models.DailyHabit", b =>
                {
                    b.HasOne("HabitsTracker.Models.AppUser", "User")
                        .WithMany("Habits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HabitsTracker.Models.HabitCompletion", b =>
                {
                    b.HasOne("HabitsTracker.Models.DailyHabit", "Habit")
                        .WithMany("Completions")
                        .HasForeignKey("HabitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Habit");
                });

            modelBuilder.Entity("HabitsTracker.Models.AppUser", b =>
                {
                    b.Navigation("Habits");
                });

            modelBuilder.Entity("HabitsTracker.Models.DailyHabit", b =>
                {
                    b.Navigation("Completions");
                });
#pragma warning restore 612, 618
        }
    }
}
