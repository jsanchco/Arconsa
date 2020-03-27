using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Table_ProfessionInClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_Client_ClientId",
                table: "Work");

            migrationBuilder.RenameIndex(
                name: "IX_Work_ClientId",
                table: "Work",
                newName: "IFK_Client_Work");

            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "UserHiring",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfessionInClient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    PriceHourSale = table.Column<decimal>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    ProfessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionInClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ProfessionInClient__ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ProfessionInClient__ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Profession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHiring_ProfessionId",
                table: "UserHiring",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IFK_Client_ProfessionInClient",
                table: "ProfessionInClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IFK_Profession_ProfessionInClient",
                table: "ProfessionInClient",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHiring_Profession_ProfessionId",
                table: "UserHiring",
                column: "ProfessionId",
                principalTable: "Profession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Work__ClientId",
                table: "Work",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHiring_Profession_ProfessionId",
                table: "UserHiring");

            migrationBuilder.DropForeignKey(
                name: "FK__Work__ClientId",
                table: "Work");

            migrationBuilder.DropTable(
                name: "ProfessionInClient");

            migrationBuilder.DropIndex(
                name: "IX_UserHiring_ProfessionId",
                table: "UserHiring");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "UserHiring");

            migrationBuilder.RenameIndex(
                name: "IFK_Client_Work",
                table: "Work",
                newName: "IX_Work_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Client_ClientId",
                table: "Work",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
