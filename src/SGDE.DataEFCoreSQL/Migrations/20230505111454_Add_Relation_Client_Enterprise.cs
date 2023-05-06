using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Relation_Client_Enterprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnterpriseId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Enterprise_Client",
                table: "Client",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK__Client__EnerpriseId",
                table: "Client",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Client__EnerpriseId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IFK_Enterprise_Client",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "Client");
        }
    }
}
