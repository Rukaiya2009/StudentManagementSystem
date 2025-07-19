using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Teachers",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ClassroomId",
                table: "Courses",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Classrooms_ClassroomId",
                table: "Courses",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DepartmentId",
                table: "Teachers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Classrooms_ClassroomId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DepartmentId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ClassroomId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Teachers",
                newName: "DepartmentName");
        }
    }
}
