using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamatetsuScheduler.Api.Migrations
{
    /// <inheritdoc />
    public partial class bacup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "target",
                table: "schedules");

            migrationBuilder.AddColumn<int>(
                name: "month",
                table: "schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "month",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "year",
                table: "schedules");

            migrationBuilder.AddColumn<DateTime>(
                name: "target",
                table: "schedules",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
