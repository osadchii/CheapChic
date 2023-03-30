using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChannelsAndMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelegramChannel",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramChannel_TelegramUser_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelegramMessage",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageId = table.Column<int>(type: "integer", nullable: false),
                    TelegramBotId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramMessage_TelegramBot_TelegramBotId",
                        column: x => x.TelegramBotId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramBot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TelegramMessage_TelegramChannel_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramChannel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TelegramMessage_TelegramUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChannel_ChatId",
                schema: "CheapChic",
                table: "TelegramChannel",
                column: "ChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramChannel_OwnerId",
                schema: "CheapChic",
                table: "TelegramChannel",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramMessage_ChannelId",
                schema: "CheapChic",
                table: "TelegramMessage",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramMessage_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramMessage",
                column: "TelegramBotId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramMessage_UserId",
                schema: "CheapChic",
                table: "TelegramMessage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramMessage_UserId_ChannelId_MessageId",
                schema: "CheapChic",
                table: "TelegramMessage",
                columns: new[] { "UserId", "ChannelId", "MessageId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramMessage",
                schema: "CheapChic");

            migrationBuilder.DropTable(
                name: "TelegramChannel",
                schema: "CheapChic");
        }
    }
}
