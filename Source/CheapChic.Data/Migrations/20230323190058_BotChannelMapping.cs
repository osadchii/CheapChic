using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class BotChannelMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramChannel_TelegramUser_OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel");

            migrationBuilder.DropForeignKey(
                name: "FK_TelegramMessage_TelegramBot_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUserState_TelegramBot_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState");

            migrationBuilder.DropIndex(
                name: "IX_TelegramChannel_OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel");

            migrationBuilder.RenameColumn(
                name: "TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                newName: "BotId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUserState_UserId_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                newName: "IX_TelegramUserState_UserId_BotId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUserState_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                newName: "IX_TelegramUserState_BotId");

            migrationBuilder.RenameColumn(
                name: "TelegramBotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                newName: "BotId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramMessage_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                newName: "IX_TelegramMessage_BotId");

            migrationBuilder.AddColumn<string>(
                name: "ChannelName",
                schema: "CheapChic",
                table: "TelegramChannel",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TelegramBotChannelMapping",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BotId = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramBotId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramBotChannelMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramBotChannelMapping_TelegramBot_TelegramBotId",
                        column: x => x.TelegramBotId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramBot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TelegramBotChannelMapping_TelegramChannel_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotChannelMapping_BotId_ChannelId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                columns: new[] { "BotId", "ChannelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotChannelMapping_ChannelId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotChannelMapping_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramBotChannelMapping",
                column: "TelegramBotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramMessage_TelegramBot_BotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                column: "BotId",
                principalSchema: "CheapChic",
                principalTable: "TelegramBot",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUserState_TelegramBot_BotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                column: "BotId",
                principalSchema: "CheapChic",
                principalTable: "TelegramBot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramMessage_TelegramBot_BotId",
                schema: "CheapChic",
                table: "TelegramMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUserState_TelegramBot_BotId",
                schema: "CheapChic",
                table: "TelegramUserState");

            migrationBuilder.DropTable(
                name: "TelegramBotChannelMapping",
                schema: "CheapChic");

            migrationBuilder.DropColumn(
                name: "ChannelName",
                schema: "CheapChic",
                table: "TelegramChannel");

            migrationBuilder.RenameColumn(
                name: "BotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                newName: "TelegramBotId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUserState_UserId_BotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                newName: "IX_TelegramUserState_UserId_TelegramBotId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUserState_BotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                newName: "IX_TelegramUserState_TelegramBotId");

            migrationBuilder.RenameColumn(
                name: "BotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                newName: "TelegramBotId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramMessage_BotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                newName: "IX_TelegramMessage_TelegramBotId");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChannel_OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramChannel_TelegramUser_OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel",
                column: "OwnerId",
                principalSchema: "CheapChic",
                principalTable: "TelegramUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramMessage_TelegramBot_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                column: "TelegramBotId",
                principalSchema: "CheapChic",
                principalTable: "TelegramBot",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUserState_TelegramBot_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                column: "TelegramBotId",
                principalSchema: "CheapChic",
                principalTable: "TelegramBot",
                principalColumn: "Id");
        }
    }
}
