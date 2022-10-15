using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Field_DateWarning_To_Table_CompanyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateWarning",
                table: "CompanyData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateWarning",
                table: "CompanyData");
        }
    }
}
