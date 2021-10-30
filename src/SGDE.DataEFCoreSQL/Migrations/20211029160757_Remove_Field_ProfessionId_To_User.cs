using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Remove_Field_ProfessionId_To_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__User__ProfessionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IFK_Profession_User",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Profession_User",
                table: "User",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK__User__ProfessionId",
                table: "User",
                column: "ProfessionId",
                principalTable: "Profession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
