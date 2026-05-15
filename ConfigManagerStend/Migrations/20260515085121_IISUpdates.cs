using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigManagerStend.Migrations
{
    /// <inheritdoc />
    public partial class IISUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssSrvName",
                table: "Stands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssWebName",
                table: "Stands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssSrvName",
                table: "Stands");

            migrationBuilder.DropColumn(
                name: "IssWebName",
                table: "Stands");
        }
    }
}
