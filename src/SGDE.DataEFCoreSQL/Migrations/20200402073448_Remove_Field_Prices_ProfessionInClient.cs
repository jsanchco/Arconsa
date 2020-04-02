using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Remove_Field_Prices_ProfessionInClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourExtra",
                table: "ProfessionInClient",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourFestive",
                table: "ProfessionInClient",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHourOrdinary",
                table: "ProfessionInClient",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
