using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentIdTypeInPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_StudentId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StudentId",
                table: "Payments",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_StudentId",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StudentId1",
                table: "Payments",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Students_StudentId1",
                table: "Payments",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "StudentId");
        }
    }
}
