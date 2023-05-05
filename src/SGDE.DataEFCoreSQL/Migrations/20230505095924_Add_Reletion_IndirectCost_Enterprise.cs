using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Reletion_IndirectCost_Enterprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnterpriseId",
                table: "IndirectCost",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Enterprise_IndirectCost",
                table: "IndirectCost",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK__IndirectCost__EnerpriseId",
                table: "IndirectCost",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__IndirectCost__EnerpriseId",
                table: "IndirectCost");

            migrationBuilder.DropIndex(
                name: "IFK_Enterprise_IndirectCost",
                table: "IndirectCost");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "IndirectCost");
        }
    }
}
