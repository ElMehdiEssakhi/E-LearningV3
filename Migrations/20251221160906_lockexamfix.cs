using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningV3.Migrations
{
    /// <inheritdoc />
    public partial class lockexamfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamFinales_CourseId",
                table: "ExamFinales");

            migrationBuilder.CreateIndex(
                name: "IX_ExamFinales_CourseId",
                table: "ExamFinales",
                column: "CourseId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamFinales_CourseId",
                table: "ExamFinales");

            migrationBuilder.CreateIndex(
                name: "IX_ExamFinales_CourseId",
                table: "ExamFinales",
                column: "CourseId");
        }
    }
}
