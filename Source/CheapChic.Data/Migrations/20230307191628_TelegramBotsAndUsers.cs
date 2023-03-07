using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramBotsAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CheapChic");

            migrationBuilder.CreateTable(
                name: "TelegramUser",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Lastname = table.Column<string>(type: "text", nullable: true),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelegramBot",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramBot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramBot_TelegramUser_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBot_OwnerId",
                schema: "CheapChic",
                table: "TelegramBot",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBot_Token",
                schema: "CheapChic",
                table: "TelegramBot",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramUser_ChatId",
                schema: "CheapChic",
                table: "TelegramUser",
                column: "ChatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramBot",
                schema: "CheapChic");

            migrationBuilder.DropTable(
                name: "TelegramUser",
                schema: "CheapChic");
        }
    }
}
