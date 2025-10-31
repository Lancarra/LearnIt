using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Users",
                type: "int",
                nullable: true);
            

            migrationBuilder.CreateTable(
                name: "TestCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdditionalAnswers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestUnits_Definitions_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestUnits_TestCards_TestCardId",
                        column: x => x.TestCardId,
                        principalTable: "TestCards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCards_UserId",
                table: "TestCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestUnits_DefinitionId",
                table: "TestUnits",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestUnits_TestCardId",
                table: "TestUnits",
                column: "TestCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestUnits");

            migrationBuilder.DropTable(
                name: "TestCards");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Users");
            
        }
    }
}
