using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelsWithDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Courses_CourseId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CourseId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaid",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentProofPath",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "proofFile",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "GPA",
                table: "Enrollments",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_StudentId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DatePaid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentProofPath",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "proofFile",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CourseId",
                table: "Payments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Courses_CourseId",
                table: "Payments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
