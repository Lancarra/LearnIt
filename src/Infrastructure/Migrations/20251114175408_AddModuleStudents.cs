using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseModules_Users_UserId",
                table: "CourseModules");

            migrationBuilder.CreateTable(
                name: "CourseModuleStudents",
                columns: table => new
                {
                    StudentCourseModulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModuleStudents", x => new { x.StudentCourseModulesId, x.StudentsUserId });
                    table.ForeignKey(
                        name: "FK_CourseModuleStudents_CourseModules_StudentCourseModulesId",
                        column: x => x.StudentCourseModulesId,
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseModuleStudents_Users_StudentsUserId",
                        column: x => x.StudentsUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseModuleStudents_StudentsUserId",
                table: "CourseModuleStudents",
                column: "StudentsUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseModules_Users_UserId",
                table: "CourseModules",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseModules_Users_UserId",
                table: "CourseModules");

            migrationBuilder.DropTable(
                name: "CourseModuleStudents");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseModules_Users_UserId",
                table: "CourseModules",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
