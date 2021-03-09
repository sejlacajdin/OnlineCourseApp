using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourseApp.Data.Migrations
{
    public partial class examansweres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamAnsweredQuestion",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordUpdated = table.Column<DateTime>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    QuestionID = table.Column<int>(nullable: false),
                    ChoiceID = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    MarkScored = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAnsweredQuestion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExamAnsweredQuestion_Choice_ChoiceID",
                        column: x => x.ChoiceID,
                        principalTable: "Choice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamAnsweredQuestion_Question_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Question",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExamAnsweredQuestion_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnsweredQuestion_ChoiceID",
                table: "ExamAnsweredQuestion",
                column: "ChoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnsweredQuestion_QuestionID",
                table: "ExamAnsweredQuestion",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnsweredQuestion_StudentID",
                table: "ExamAnsweredQuestion",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamAnsweredQuestion");
        }
    }
}
