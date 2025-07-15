using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGradeEnumColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeEnum",
                table: "Enrollments",
                type: "int",
                nullable: true);

            // Data migration: map old Grade strings to new enum values
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 1 WHERE Grade = 'A+'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 2 WHERE Grade = 'A'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 3 WHERE Grade = 'A-'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 4 WHERE Grade = 'B+'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 5 WHERE Grade = 'B'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 6 WHERE Grade = 'B-'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 7 WHERE Grade = 'C+'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 8 WHERE Grade = 'C'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 9 WHERE Grade = 'C-'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 10 WHERE Grade = 'D'");
            migrationBuilder.Sql("UPDATE Enrollments SET GradeEnum = 11 WHERE Grade = 'F'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeEnum",
                table: "Enrollments");
        }
    }
}
