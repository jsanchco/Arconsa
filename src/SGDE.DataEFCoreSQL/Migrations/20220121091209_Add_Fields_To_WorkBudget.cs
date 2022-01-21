using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Fields_To_WorkBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "WorkBudget",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "WorkBudget",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameInWork",
                table: "WorkBudget",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeFile",
                table: "WorkBudget",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "WorkBudget");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "WorkBudget");

            migrationBuilder.DropColumn(
                name: "NameInWork",
                table: "WorkBudget");

            migrationBuilder.DropColumn(
                name: "TypeFile",
                table: "WorkBudget");
        }
    }
}
