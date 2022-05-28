using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Relation_WorkBudgetDataId_To_Table_WorkBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBudget_WorkBudgetData_WorkBudgetDataId",
                table: "WorkBudget");

            migrationBuilder.RenameIndex(
                name: "IX_WorkBudget_WorkBudgetDataId",
                table: "WorkBudget",
                newName: "IFK_WorkBudgetData_WorkBudget");

            migrationBuilder.AddForeignKey(
                name: "FK__WorkBudget__WorkBudgetDataId",
                table: "WorkBudget",
                column: "WorkBudgetDataId",
                principalTable: "WorkBudgetData",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__WorkBudget__WorkBudgetDataId",
                table: "WorkBudget");

            migrationBuilder.RenameIndex(
                name: "IFK_WorkBudgetData_WorkBudget",
                table: "WorkBudget",
                newName: "IX_WorkBudget_WorkBudgetDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBudget_WorkBudgetData_WorkBudgetDataId",
                table: "WorkBudget",
                column: "WorkBudgetDataId",
                principalTable: "WorkBudgetData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
