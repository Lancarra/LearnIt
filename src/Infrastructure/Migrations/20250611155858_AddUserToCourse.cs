using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CourseModules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_UserId",
                table: "CourseModules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseModules_Users_UserId",
                table: "CourseModules",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseModules_Users_UserId",
                table: "CourseModules");

            migrationBuilder.DropIndex(
                name: "IX_CourseModules_UserId",
                table: "CourseModules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CourseModules");
        }
    }
}
