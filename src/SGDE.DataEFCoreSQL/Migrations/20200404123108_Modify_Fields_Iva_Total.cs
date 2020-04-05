using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Modify_Fields_Iva_Total : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Invoice");

            migrationBuilder.AlterColumn<bool>(
                name: "Iva",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Iva",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
