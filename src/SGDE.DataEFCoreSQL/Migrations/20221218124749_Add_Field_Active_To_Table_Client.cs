using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Field_Active_To_Table_Client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Client",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Client");
        }
    }
}
