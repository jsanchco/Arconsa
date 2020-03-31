using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Table_CostWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostWorker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    PriceHourOrdinary = table.Column<decimal>(nullable: false),
                    PriceHourExtra = table.Column<decimal>(nullable: false),
                    PriceHourFestive = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    ProfessionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostWorker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostWorker_Profession_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Profession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__CostWorker__UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostWorker_ProfessionId",
                table: "CostWorker",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IFK_User_CostClient",
                table: "CostWorker",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostWorker");
        }
    }
}
