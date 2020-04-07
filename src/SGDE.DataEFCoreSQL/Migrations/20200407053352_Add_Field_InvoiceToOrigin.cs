using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Field_InvoiceToOrigin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvoiceToOrigin",
                table: "Work",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageRetention",
                table: "Work",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "TotalContract",
                table: "Work",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceToOrigin",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "PercentageRetention",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "TotalContract",
                table: "Work");
        }
    }
}
