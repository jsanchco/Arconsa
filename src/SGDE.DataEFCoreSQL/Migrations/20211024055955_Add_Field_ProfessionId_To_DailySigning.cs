using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Field_ProfessionId_To_DailySigning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "DailySigning",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IFK_Profession_DailySigning",
                table: "DailySigning",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK__DailySigning__ProfessionId",
                table: "DailySigning",
                column: "ProfessionId",
                principalTable: "Profession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DailySigning__ProfessionId",
                table: "DailySigning");

            migrationBuilder.DropIndex(
                name: "IFK_Profession_DailySigning",
                table: "DailySigning");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "DailySigning");
        }
    }
}
