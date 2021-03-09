using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourseApp.Data.Migrations
{
    public partial class CourseGroupStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseGroup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CourseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroup", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseGroup_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CourseGroupStudent",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordUpdated = table.Column<DateTime>(nullable: false),
                    GroupID = table.Column<int>(nullable: false),
                    CourseParticipantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroupStudent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseGroupStudent_CourseParticipants_CourseParticipantID",
                        column: x => x.CourseParticipantID,
                        principalTable: "CourseParticipants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroupStudent_CourseGroup_GroupID",
                        column: x => x.GroupID,
                        principalTable: "CourseGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroup_CourseID",
                table: "CourseGroup",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupStudent_CourseParticipantID",
                table: "CourseGroupStudent",
                column: "CourseParticipantID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupStudent_GroupID",
                table: "CourseGroupStudent",
                column: "GroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseGroupStudent");

            migrationBuilder.DropTable(
                name: "CourseGroup");
        }
    }
}
