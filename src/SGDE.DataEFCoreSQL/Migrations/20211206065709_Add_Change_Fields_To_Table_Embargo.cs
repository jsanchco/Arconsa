using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Change_Fields_To_Table_Embargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Embargo");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Embargo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Embargo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Embargo");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Embargo");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Embargo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
