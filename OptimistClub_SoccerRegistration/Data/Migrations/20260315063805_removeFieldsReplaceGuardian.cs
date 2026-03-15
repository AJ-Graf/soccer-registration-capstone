using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimistClub_SoccerRegistration.Migrations
{
    /// <inheritdoc />
    public partial class removeFieldsReplaceGuardian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Guardians_GuardianId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Guardians_GuardianId",
                table: "Volunteers");

            migrationBuilder.DropTable(
                name: "Guardians");

            migrationBuilder.DropColumn(
                name: "MedicalInfo",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "GuardianId",
                table: "Volunteers",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Volunteers_GuardianId",
                table: "Volunteers",
                newName: "IX_Volunteers_ParentId");

            migrationBuilder.RenameColumn(
                name: "GuardianId",
                table: "Players",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_GuardianId",
                table: "Players",
                newName: "IX_Players_ParentId");

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    ParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    ElectronicSignature = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WaiverAccepted = table.Column<bool>(type: "bit", nullable: false),
                    WaiverAcceptedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.ParentId);
                    table.ForeignKey(
                        name: "FK_Parents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parents_UserId",
                table: "Parents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Parents_ParentId",
                table: "Players",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "ParentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Parents_ParentId",
                table: "Volunteers",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "ParentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Parents_ParentId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Parents_ParentId",
                table: "Volunteers");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Volunteers",
                newName: "GuardianId");

            migrationBuilder.RenameIndex(
                name: "IX_Volunteers_ParentId",
                table: "Volunteers",
                newName: "IX_Volunteers_GuardianId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Players",
                newName: "GuardianId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_ParentId",
                table: "Players",
                newName: "IX_Players_GuardianId");

            migrationBuilder.AddColumn<string>(
                name: "MedicalInfo",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guardians",
                columns: table => new
                {
                    GuardianId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectronicSignature = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    WaiverAccepted = table.Column<bool>(type: "bit", nullable: false),
                    WaiverAcceptedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardians", x => x.GuardianId);
                    table.ForeignKey(
                        name: "FK_Guardians_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_UserId",
                table: "Guardians",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Guardians_GuardianId",
                table: "Players",
                column: "GuardianId",
                principalTable: "Guardians",
                principalColumn: "GuardianId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Guardians_GuardianId",
                table: "Volunteers",
                column: "GuardianId",
                principalTable: "Guardians",
                principalColumn: "GuardianId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
