using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConfigManagerStend.Migrations
{
    /// <inheritdoc />
    public partial class InitAlpha : Migration
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
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    SrvAFolderPath = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    DbProvider = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ChekedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameProject = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StandId = table.Column<int>(type: "INTEGER", nullable: false),
                    FullPathFile = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    DateFileReplacement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFileVerifiedToExist = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalModules_ConfigStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ConfigStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalModules_Stands_StandId",
                        column: x => x.StandId,
                        principalTable: "Stands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameRepo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildDefinitions_TeamProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "TeamProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigStends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameConfig = table.Column<string>(type: "TEXT", nullable: false),
                    Appn = table.Column<string>(type: "TEXT", nullable: false),
                    PathStend = table.Column<string>(type: "TEXT", nullable: false),
                    PortA = table.Column<int>(type: "INTEGER", nullable: false),
                    DbOwner = table.Column<string>(type: "TEXT", nullable: false),
                    PlatformHostDir = table.Column<string>(type: "TEXT", nullable: false),
                    BoxSettingDir = table.Column<string>(type: "TEXT", nullable: false),
                    RepoId = table.Column<int>(type: "INTEGER", nullable: false),
                    BranchName = table.Column<string>(type: "TEXT", nullable: false),
                    SettingDb = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigStends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigStends_BuildDefinitions_RepoId",
                        column: x => x.RepoId,
                        principalTable: "BuildDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ConfigStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Подключен" },
                    { 2, "Отключен" }
                });

            migrationBuilder.InsertData(
                table: "TeamProjects",
                columns: new[] { "Id", "NameProject" },
                values: new object[,]
                {
                    { 1, "Delo2020" },
                    { 2, "TITUL" },
                    { 3, "Nadzor2025" }
                });

            migrationBuilder.InsertData(
                table: "BuildDefinitions",
                columns: new[] { "Id", "NameRepo", "ProjectId" },
                values: new object[,]
                {
                    { 1, "Delo2020", 1 },
                    { 2, "Delo2020-CB", 1 },
                    { 3, "Delo2020-EEK", 1 },
                    { 4, "Delo2020-GD", 1 },
                    { 5, "Delo2020-Kaliningrad", 1 },
                    { 6, "Delo2020-MinKult", 1 },
                    { 7, "Delo2020-MinTrud", 1 },
                    { 8, "TITUL", 2 },
                    { 9, "Nadzor2025", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildDefinitions_ProjectId",
                table: "BuildDefinitions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigStends_RepoId",
                table: "ConfigStends",
                column: "RepoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalModules_StandId",
                table: "ExternalModules",
                column: "StandId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalModules_StatusId",
                table: "ExternalModules",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigStends");

            migrationBuilder.DropTable(
                name: "ExternalModules");

            migrationBuilder.DropTable(
                name: "BuildDefinitions");

            migrationBuilder.DropTable(
                name: "ConfigStatuses");

            migrationBuilder.DropTable(
                name: "Stands");

            migrationBuilder.DropTable(
                name: "TeamProjects");
        }
    }
}
