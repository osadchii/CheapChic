using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhotoHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                schema: "CheapChic",
                table: "Photo",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Hash",
                schema: "CheapChic",
                table: "Photo",
                column: "Hash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photo_Hash",
                schema: "CheapChic",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "Hash",
                schema: "CheapChic",
                table: "Photo");
        }
    }
}
