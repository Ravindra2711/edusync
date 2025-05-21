using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResultEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "DateAwarded",
                table: "Results",
                newName: "AttemptDate");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "Results",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Results",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "AttemptDate",
                table: "Results",
                newName: "DateAwarded");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "Results",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Results",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
