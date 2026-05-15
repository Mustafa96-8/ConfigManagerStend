using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigManagerStend.Migrations
{
    /// <inheritdoc />
    public partial class IISUpdates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssWebName",
                table: "Stands",
                newName: "IisWebName");

            migrationBuilder.RenameColumn(
                name: "IssSrvName",
                table: "Stands",
                newName: "IisSrvName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IisWebName",
                table: "Stands",
                newName: "IssWebName");

            migrationBuilder.RenameColumn(
                name: "IisSrvName",
                table: "Stands",
                newName: "IssSrvName");
        }
    }
}
