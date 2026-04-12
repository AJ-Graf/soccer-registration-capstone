using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimistClub_SoccerRegistration.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationOpenCloseDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationOpenDate",
                table: "RegistrationPeriods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationCloseDate",
                table: "RegistrationPeriods",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationOpenDate",
                table: "RegistrationPeriods");

            migrationBuilder.DropColumn(
                name: "RegistrationCloseDate",
                table: "RegistrationPeriods");
        }
    }
}
