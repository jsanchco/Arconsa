using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Add_Table_InvoicePaymentsHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPayment",
                table: "Invoice",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "InvoicePaymentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    DatePayment = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Observations = table.Column<string>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePaymentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK__InvoicePaymentHistory__InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IFK_Invoice_InvoicePaymentHistory",
                table: "InvoicePaymentHistory",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoicePaymentHistory");

            migrationBuilder.DropColumn(
                name: "TotalPayment",
                table: "Invoice");
        }
    }
}
