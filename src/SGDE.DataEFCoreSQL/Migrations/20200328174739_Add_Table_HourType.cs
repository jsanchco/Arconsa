using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Table_HourType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDocument_TypeDocument_TypeDocumentId",
                table: "UserDocument");

            migrationBuilder.RenameIndex(
                name: "IX_UserDocument_TypeDocumentId",
                table: "UserDocument",
                newName: "IFK_TypeDocument_UserDocument");

            migrationBuilder.AddColumn<int>(
                name: "HourTypeId",
                table: "DailySigning",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HourType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IFK_HourType_DailySigning",
                table: "DailySigning",
                column: "HourTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK__DailySigning__HourTypeId",
                table: "DailySigning",
                column: "HourTypeId",
                principalTable: "HourType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__UserDocument__TypeDocumentId",
                table: "UserDocument",
                column: "TypeDocumentId",
                principalTable: "TypeDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DailySigning__HourTypeId",
                table: "DailySigning");

            migrationBuilder.DropForeignKey(
                name: "FK__UserDocument__TypeDocumentId",
                table: "UserDocument");

            migrationBuilder.DropTable(
                name: "HourType");

            migrationBuilder.DropIndex(
                name: "IFK_HourType_DailySigning",
                table: "DailySigning");

            migrationBuilder.DropColumn(
                name: "HourTypeId",
                table: "DailySigning");

            migrationBuilder.RenameIndex(
                name: "IFK_TypeDocument_UserDocument",
                table: "UserDocument",
                newName: "IX_UserDocument_TypeDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocument_TypeDocument_TypeDocumentId",
                table: "UserDocument",
                column: "TypeDocumentId",
                principalTable: "TypeDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
