using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Fields_Email_To_Table_Client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailInvoice",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "EmailInvoice",
                table: "Client");
        }
    }
}
