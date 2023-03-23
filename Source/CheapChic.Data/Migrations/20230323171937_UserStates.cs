using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "CheapChic",
                table: "TelegramBot",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TelegramUserState",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramBotId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramUserState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramUserState_TelegramBot_TelegramBotId",
                        column: x => x.TelegramBotId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramBot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TelegramUserState_TelegramUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUserState_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                column: "TelegramBotId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUserState_UserId_TelegramBotId",
                schema: "CheapChic",
                table: "TelegramUserState",
                columns: new[] { "UserId", "TelegramBotId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramUserState",
                schema: "CheapChic");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "CheapChic",
                table: "TelegramBot");
        }
    }
}
