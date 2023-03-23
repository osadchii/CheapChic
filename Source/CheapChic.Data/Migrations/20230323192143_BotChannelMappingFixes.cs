using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class BotChannelMappingFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramBotChannelMapping_TelegramBot_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping");

            migrationBuilder.DropIndex(
                name: "IX_TelegramBotChannelMapping_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping");

            migrationBuilder.DropColumn(
                name: "TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramBotChannelMapping_TelegramBot_BotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                column: "BotId",
                principalSchema: "CheapChic",
                principalTable: "TelegramBot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramBotChannelMapping_TelegramBot_BotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping");

            migrationBuilder.AddColumn<Guid>(
                name: "TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotChannelMapping_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                column: "TelegramBotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramBotChannelMapping_TelegramBot_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                column: "TelegramBotId",
                principalSchema: "CheapChic",
                principalTable: "TelegramBot",
                principalColumn: "Id");
        }
    }
}
