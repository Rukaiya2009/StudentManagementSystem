using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenderToEnumWithDataConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, add a temporary column
            migrationBuilder.AddColumn<int>(
                name: "GenderTemp",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Convert existing string values to enum values
            migrationBuilder.Sql(@"
                UPDATE Students 
                SET GenderTemp = 
                    CASE 
                        WHEN Gender = 'Male' THEN 0
                        WHEN Gender = 'Female' THEN 1
                        WHEN Gender = 'Other' THEN 2
                        ELSE 0
                    END
            ");

            // Drop the old column
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");

            // Rename the temporary column to Gender
            migrationBuilder.RenameColumn(
                name: "GenderTemp",
                table: "Students",
                newName: "Gender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add a temporary string column
            migrationBuilder.AddColumn<string>(
                name: "GenderTemp",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Male");

            // Convert enum values back to strings
            migrationBuilder.Sql(@"
                UPDATE Students 
                SET GenderTemp = 
                    CASE 
                        WHEN Gender = 0 THEN 'Male'
                        WHEN Gender = 1 THEN 'Female'
                        WHEN Gender = 2 THEN 'Other'
                        ELSE 'Male'
                    END
            ");

            // Drop the old column
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");

            // Rename the temporary column to Gender
            migrationBuilder.RenameColumn(
                name: "GenderTemp",
                table: "Students",
                newName: "Gender");
        }
    }
}
