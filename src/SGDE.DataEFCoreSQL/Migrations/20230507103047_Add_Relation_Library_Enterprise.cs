using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Relation_Library_Enterprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnterpriseId",
                table: "Library",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Enterprise_Library",
                table: "Library",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK__Library__EnerpriseId",
                table: "Library",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Library__EnerpriseId",
                table: "Library");

            migrationBuilder.DropIndex(
                name: "IFK_Enterprise_Library",
                table: "Library");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "Library");
        }
    }
}
