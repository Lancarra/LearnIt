using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDefinitionBlobType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobURL",
                table: "Definitions");

            migrationBuilder.AddColumn<Guid>(
                name: "BlobId",
                table: "Definitions",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobId",
                table: "Definitions");

            migrationBuilder.AddColumn<string>(
                name: "BlobURL",
                table: "Definitions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
