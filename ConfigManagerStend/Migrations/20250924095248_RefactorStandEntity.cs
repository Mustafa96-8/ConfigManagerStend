using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigManagerStend.Migrations
{
    /// <inheritdoc />
    public partial class RefactorStandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppName",
                table: "Stands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppPort",
                table: "Stands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppUrl",
                table: "Stands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DbOwner",
                table: "Stands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DateFileVerifiedToExist",
                table: "ExternalModules",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppName",
                table: "Stands");

            migrationBuilder.DropColumn(
                name: "AppPort",
                table: "Stands");

            migrationBuilder.DropColumn(
                name: "AppUrl",
                table: "Stands");

            migrationBuilder.DropColumn(
                name: "DbOwner",
                table: "Stands");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFileVerifiedToExist",
                table: "ExternalModules",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
