using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Fields_Prices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceHourSale",
                table: "ProfessionInClient");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourExtra",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourFestive",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourOrdinary",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourSaleExtra",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourSaleFestive",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourSaleOrdinary",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceHourExtra",
                table: "ProfessionInClient");

            migrationBuilder.DropColumn(
                name: "PriceHourFestive",
                table: "ProfessionInClient");

            migrationBuilder.DropColumn(
                name: "PriceHourOrdinary",
                table: "ProfessionInClient");

            migrationBuilder.DropColumn(
                name: "PriceHourSaleExtra",
                table: "ProfessionInClient");

            migrationBuilder.DropColumn(
                name: "PriceHourSaleFestive",
                table: "ProfessionInClient");

            migrationBuilder.DropColumn(
                name: "PriceHourSaleOrdinary",
                table: "ProfessionInClient");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourSale",
                table: "ProfessionInClient",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
