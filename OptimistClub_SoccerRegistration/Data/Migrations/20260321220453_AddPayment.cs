using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimistClub_SoccerRegistration.Migrations
{
    /// <inheritdoc />
    public partial class AddPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_RegistrationPeriods_PeriodId",
                table: "Registrations");

            migrationBuilder.AlterColumn<int>(
                name: "PeriodId",
                table: "Registrations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Parents",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_RegistrationPeriods_PeriodId",
                table: "Registrations",
                column: "PeriodId",
                principalTable: "RegistrationPeriods",
                principalColumn: "PeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_RegistrationPeriods_PeriodId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Parents");

            migrationBuilder.AlterColumn<int>(
                name: "PeriodId",
                table: "Registrations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_RegistrationPeriods_PeriodId",
                table: "Registrations",
                column: "PeriodId",
                principalTable: "RegistrationPeriods",
                principalColumn: "PeriodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
