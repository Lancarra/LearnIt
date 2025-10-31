using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestCardIdNewRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TestCardId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TestCardId",
                table: "Users",
                column: "TestCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TestCards_TestCardId",
                table: "Users",
                column: "TestCardId",
                principalTable: "TestCards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_TestCards_TestCardId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TestCardId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TestCardId",
                table: "Users");
        }
    }
}
