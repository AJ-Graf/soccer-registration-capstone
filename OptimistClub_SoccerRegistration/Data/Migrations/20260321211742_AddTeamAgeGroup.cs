using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimistClub_SoccerRegistration.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamAgeGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgeGroup",
                table: "Teams",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeGroup",
                table: "Teams");
        }
    }
}
