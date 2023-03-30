using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheapChic.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdsAndPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ad",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BotId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(3072)", maxLength: 3072, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateOfLastPublication = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Disable = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ad_TelegramBot_BotId",
                        column: x => x.BotId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramBot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ad_TelegramUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "CheapChic",
                        principalTable: "TelegramUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdPhoto",
                schema: "CheapChic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhotoId = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdPhoto_Ad_AdId",
                        column: x => x.AdId,
                        principalSchema: "CheapChic",
                        principalTable: "Ad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ad_BotId",
                schema: "CheapChic",
                table: "Ad",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_Ad_DateOfLastPublication",
                schema: "CheapChic",
                table: "Ad",
                column: "DateOfLastPublication");

            migrationBuilder.CreateIndex(
                name: "IX_Ad_UserId",
                schema: "CheapChic",
                table: "Ad",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdPhoto_AdId",
                schema: "CheapChic",
                table: "AdPhoto",
                column: "AdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdPhoto",
                schema: "CheapChic");

            migrationBuilder.DropTable(
                name: "Photo",
                schema: "CheapChic");

            migrationBuilder.DropTable(
                name: "Ad",
                schema: "CheapChic");
        }
    }
}
