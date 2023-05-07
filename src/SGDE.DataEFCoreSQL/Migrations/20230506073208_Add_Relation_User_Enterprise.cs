using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Relation_User_Enterprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnterpriseId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Enterprise_User",
                table: "User",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK__User__EnerpriseId",
                table: "User",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__User__EnerpriseId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IFK_Enterprise_User",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "User");
        }
    }
}
