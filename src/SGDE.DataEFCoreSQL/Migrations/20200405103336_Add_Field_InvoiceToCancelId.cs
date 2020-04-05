using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Field_InvoiceToCancelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceToCancelId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IFK_Invoice_Invoice",
                table: "Invoice",
                column: "InvoiceToCancelId");

            migrationBuilder.AddForeignKey(
                name: "FK__Invoice__InvoiceToCancelId",
                table: "Invoice",
                column: "InvoiceToCancelId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Invoice__InvoiceToCancelId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IFK_Invoice_Invoice",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "InvoiceToCancelId",
                table: "Invoice");
        }
    }
}
