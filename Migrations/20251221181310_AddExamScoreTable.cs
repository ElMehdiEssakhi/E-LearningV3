using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningV3.Migrations
{
    /// <inheritdoc />
    public partial class AddExamScoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamScores",
                columns: table => new
                {
                    ExamScoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamFinalId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ScoreValue = table.Column<int>(type: "int", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamScores", x => x.ExamScoreId);
                    table.ForeignKey(
                        name: "FK_ExamScores_ExamFinales_ExamFinalId",
                        column: x => x.ExamFinalId,
                        principalTable: "ExamFinales",
                        principalColumn: "ExamFinalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamScores_ExamFinalId",
                table: "ExamScores",
                column: "ExamFinalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamScores");
        }
    }
}
