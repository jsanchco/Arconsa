using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Reletion_CompanyData_Enterprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnterpriseId",
                table: "CompanyData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Enterprise_CompanyData",
                table: "CompanyData",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK__CompanyData__EnerpriseId",
                table: "CompanyData",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__CompanyData__EnerpriseId",
                table: "CompanyData");

            migrationBuilder.DropIndex(
                name: "IFK_Enterprise_CompanyData",
                table: "CompanyData");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "CompanyData");
        }
    }
}
