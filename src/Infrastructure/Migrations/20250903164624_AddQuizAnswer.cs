using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestCardAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCardAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCardAnswer_TestCards_TestCardId",
                        column: x => x.TestCardId,
                        principalTable: "TestCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestCardAnswer_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TestUnitAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestCardAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUnitAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestUnitAnswers_TestCardAnswer_TestCardAnswerId",
                        column: x => x.TestCardAnswerId,
                        principalTable: "TestCardAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestUnitAnswers_TestUnits_TestUnitId",
                        column: x => x.TestUnitId,
                        principalTable: "TestUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCardAnswer_TestCardId",
                table: "TestCardAnswer",
                column: "TestCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCardAnswer_UserId",
                table: "TestCardAnswer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestUnitAnswers_TestCardAnswerId",
                table: "TestUnitAnswers",
                column: "TestCardAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestUnitAnswers_TestUnitId",
                table: "TestUnitAnswers",
                column: "TestUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestUnitAnswers");

            migrationBuilder.DropTable(
                name: "TestCardAnswer");
        }
    }
}
