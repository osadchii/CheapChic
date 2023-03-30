using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class BotSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "CheapChic",
                table: "TelegramBot",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublishEveryHours",
                schema: "CheapChic",
                table: "TelegramBot",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublishForDays",
                schema: "CheapChic",
                table: "TelegramBot",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "CheapChic",
                table: "TelegramBot");

            migrationBuilder.DropColumn(
                name: "PublishEveryHours",
                schema: "CheapChic",
                table: "TelegramBot");

            migrationBuilder.DropColumn(
                name: "PublishForDays",
                schema: "CheapChic",
                table: "TelegramBot");
        }
    }
}
