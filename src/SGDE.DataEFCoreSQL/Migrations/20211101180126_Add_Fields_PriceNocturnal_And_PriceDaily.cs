using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Fields_PriceNocturnal_And_PriceDaily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceDaily",
                table: "CostWorker",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourNocturnal",
                table: "CostWorker",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceDaily",
                table: "CostWorker");

            migrationBuilder.DropColumn(
                name: "PriceHourNocturnal",
                table: "CostWorker");
        }
    }
}
