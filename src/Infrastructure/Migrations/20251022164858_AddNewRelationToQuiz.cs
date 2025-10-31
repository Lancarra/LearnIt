using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRelationToQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DictionaryId",
                table: "TestCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestCards_DictionaryId",
                table: "TestCards",
                column: "DictionaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCards_LearnWordDictionaries_DictionaryId",
                table: "TestCards",
                column: "DictionaryId",
                principalTable: "LearnWordDictionaries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCards_LearnWordDictionaries_DictionaryId",
                table: "TestCards");

            migrationBuilder.DropIndex(
                name: "IX_TestCards_DictionaryId",
                table: "TestCards");

            migrationBuilder.DropColumn(
                name: "DictionaryId",
                table: "TestCards");
        }
    }
}
