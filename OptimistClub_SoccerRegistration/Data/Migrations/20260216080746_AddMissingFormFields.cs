using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimistClub_SoccerRegistration.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFormFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CriminalCheckCompleted",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShirtSize",
                table: "Volunteers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShirtSize",
                table: "Players",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "Players",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriminalCheckCompleted",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "ShirtSize",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "ShirtSize",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Town",
                table: "Players");
        }
    }
}
