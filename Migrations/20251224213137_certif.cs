using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningV3.Migrations
{
    /// <inheritdoc />
    public partial class certif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Certificates",
                newName: "FilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Certificates",
                newName: "Link");
        }
    }
}
