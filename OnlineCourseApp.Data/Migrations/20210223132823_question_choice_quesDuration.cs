using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourseApp.Data.Migrations
{
    public partial class question_choice_quesDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordUpdated = table.Column<DateTime>(nullable: false),
                    QuestionCategoryID = table.Column<int>(nullable: false),
                    QuestionType = table.Column<int>(nullable: false),
                    ExamID = table.Column<int>(nullable: false),
                    Points = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Question_Exam_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exam",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_QuestionCategory_QuestionCategoryID",
                        column: x => x.QuestionCategoryID,
                        principalTable: "QuestionCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choice",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordUpdated = table.Column<DateTime>(nullable: false),
                    QuestionID = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Points = table.Column<double>(nullable: false),
                    IsCorrect = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Choice_Question_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Question",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDuration",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordUpdated = table.Column<DateTime>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    QuestionID = table.Column<int>(nullable: false),
                    RequestTime = table.Column<TimeSpan>(nullable: false),
                    LeaveTime = table.Column<TimeSpan>(nullable: false),
                    AnsweredTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDuration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuestionDuration_Question_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Question",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionDuration_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choice_QuestionID",
                table: "Choice",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ExamID",
                table: "Question",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionCategoryID",
                table: "Question",
                column: "QuestionCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDuration_QuestionID",
                table: "QuestionDuration",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDuration_StudentID",
                table: "QuestionDuration",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Choice");

            migrationBuilder.DropTable(
                name: "QuestionDuration");

            migrationBuilder.DropTable(
                name: "Question");
        }
    }
}
