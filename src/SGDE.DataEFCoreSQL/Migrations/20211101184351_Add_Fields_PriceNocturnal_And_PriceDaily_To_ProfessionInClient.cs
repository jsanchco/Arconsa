using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Fields_PriceNocturnal_And_PriceDaily_To_ProfessionInClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceDailySale",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourSaleNocturnal",
                table: "ProfessionInClient",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceDailySale",
                table: "ProfessionInClient");

            migrationBuilder.DropColumn(
                name: "PriceHourSaleNocturnal",
                table: "ProfessionInClient");
        }
    }
}
