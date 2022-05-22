using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Table_WorkBudgetData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkBudgetDataId",
                table: "WorkBudget",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkBudgetData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    WorkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkBudgetData", x => x.Id);
                    table.ForeignKey(
                        name: "FK__WorkBudgetData__WorkId",
                        column: x => x.WorkId,
                        principalTable: "Work",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkBudget_WorkBudgetDataId",
                table: "WorkBudget",
                column: "WorkBudgetDataId");

            migrationBuilder.CreateIndex(
                name: "IFK_Work_WorkBudgetData",
                table: "WorkBudgetData",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBudget_WorkBudgetData_WorkBudgetDataId",
                table: "WorkBudget",
                column: "WorkBudgetDataId",
                principalTable: "WorkBudgetData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBudget_WorkBudgetData_WorkBudgetDataId",
                table: "WorkBudget");

            migrationBuilder.DropTable(
                name: "WorkBudgetData");

            migrationBuilder.DropIndex(
                name: "IX_WorkBudget_WorkBudgetDataId",
                table: "WorkBudget");

            migrationBuilder.DropColumn(
                name: "WorkBudgetDataId",
                table: "WorkBudget");
        }
    }
}
