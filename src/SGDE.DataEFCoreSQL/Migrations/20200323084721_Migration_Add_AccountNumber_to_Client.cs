using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Migration_Add_AccountNumber_to_Client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WayToPay",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "WayToPay",
                table: "Client");
        }
    }
}
