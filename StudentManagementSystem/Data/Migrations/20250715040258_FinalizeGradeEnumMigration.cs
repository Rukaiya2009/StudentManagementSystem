using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeGradeEnumMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old string Grade column
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Enrollments");

            // Rename GradeEnum to Grade
            migrationBuilder.RenameColumn(
                name: "GradeEnum",
                table: "Enrollments",
                newName: "Grade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rename Grade back to GradeEnum
            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Enrollments",
                newName: "GradeEnum");

            // Add back the old string Grade column (nullable for safety)
            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Enrollments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
