using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitsTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddHabitTargetTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TargetTime",
                table: "Habits",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetTime",
                table: "Habits");
        }
    }
}
