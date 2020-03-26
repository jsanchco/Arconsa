using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Field_ProfessionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHiring_Profession_ProfessionId",
                table: "UserHiring");

            migrationBuilder.RenameIndex(
                name: "IX_UserHiring_ProfessionId",
                table: "UserHiring",
                newName: "IFK_Profession_UserHiring");

            migrationBuilder.AddForeignKey(
                name: "FK__UserHiring__ProfessionId",
                table: "UserHiring",
                column: "ProfessionId",
                principalTable: "Profession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserHiring__ProfessionId",
                table: "UserHiring");

            migrationBuilder.RenameIndex(
                name: "IFK_Profession_UserHiring",
                table: "UserHiring",
                newName: "IX_UserHiring_ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHiring_Profession_ProfessionId",
                table: "UserHiring",
                column: "ProfessionId",
                principalTable: "Profession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
