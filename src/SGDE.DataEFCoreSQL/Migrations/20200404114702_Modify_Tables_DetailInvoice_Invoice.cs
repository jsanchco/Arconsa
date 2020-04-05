using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGDE.DataEFCoreSQL.Migrations
{
    public partial class Modify_Tables_DetailInvoice_Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Invoice__WorkId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "WorkId",
                table: "Invoice",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Invoice",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ClientId",
                table: "Invoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_UserId",
                table: "Invoice",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Client_ClientId",
                table: "Invoice",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_User_UserId",
                table: "Invoice",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Invoice__WorkId",
                table: "Invoice",
                column: "WorkId",
                principalTable: "Work",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Client_ClientId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_User_UserId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK__Invoice__WorkId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ClientId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_UserId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "WorkId",
                table: "Invoice",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Invoice__WorkId",
                table: "Invoice",
                column: "WorkId",
                principalTable: "Work",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
