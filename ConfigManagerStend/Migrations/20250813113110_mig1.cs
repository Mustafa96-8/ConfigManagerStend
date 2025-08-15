using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConfigManagerStend.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameStatus = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameStand = table.Column<string>(type: "TEXT", nullable: false),
                    FullPathFile = table.Column<string>(type: "TEXT", nullable: false),
                    NameFile = table.Column<string>(type: "TEXT", nullable: false),
                    DateFileReplacement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFileVerifiedToExist = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configs_ConfigStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ConfigStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ConfigStatuses",
                columns: new[] { "Id", "NameStatus" },
                values: new object[,]
                {
                    { 1, "Подключен" },
                    { 2, "Отключен" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_StatusId",
                table: "Configs",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "ConfigStatuses");
        }
    }
}
