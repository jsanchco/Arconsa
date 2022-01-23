using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_WorkBudget_to_Table_Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkBudgetId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_WorkBudget_Invoice",
                table: "Invoice",
                column: "WorkBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK__Invoice__WorkBudgetId",
                table: "Invoice",
                column: "WorkBudgetId",
                principalTable: "WorkBudget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Invoice__WorkBudgetId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IFK_WorkBudget_Invoice",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "WorkBudgetId",
                table: "Invoice");
        }
    }
}
