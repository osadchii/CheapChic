using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class GuidPhotoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                schema: "CheapChic",
                table: "AdPhoto",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AdPhoto_PhotoId",
                schema: "CheapChic",
                table: "AdPhoto",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdPhoto_Photo_PhotoId",
                schema: "CheapChic",
                table: "AdPhoto",
                column: "PhotoId",
                principalSchema: "CheapChic",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdPhoto_Photo_PhotoId",
                schema: "CheapChic",
                table: "AdPhoto");

            migrationBuilder.DropIndex(
                name: "IX_AdPhoto_PhotoId",
                schema: "CheapChic",
                table: "AdPhoto");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                schema: "CheapChic",
                table: "AdPhoto");
        }
    }
}
