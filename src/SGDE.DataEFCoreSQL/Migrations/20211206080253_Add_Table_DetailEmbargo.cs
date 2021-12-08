using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Table_DetailEmbargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Embargo",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DetailEmbargo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    DatePay = table.Column<DateTime>(nullable: false),
                    Observations = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmbargoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailEmbargo", x => x.Id);
                    table.ForeignKey(
                        name: "FK__DetailEmbargo__EmbargoId",
                        column: x => x.EmbargoId,
                        principalTable: "Embargo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IFK_Embargo_DetailEmbargo",
                table: "DetailEmbargo",
                column: "EmbargoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailEmbargo");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Embargo");
        }
    }
}
