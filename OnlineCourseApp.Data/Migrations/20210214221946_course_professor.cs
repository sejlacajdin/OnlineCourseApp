using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourseApp.Data.Migrations
{
    public partial class course_professor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Course",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_ProfessorId",
                table: "Course",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_AspNetUsers_ProfessorId",
                table: "Course",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_AspNetUsers_ProfessorId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_ProfessorId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Course");
        }
    }
}
