using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningV3.Migrations
{
    /// <inheritdoc />
    public partial class lockexam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "ExamFinales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "ExamFinales");
        }
    }
}
