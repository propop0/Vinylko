using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSaleForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_vinyl_records_RecordId",
                table: "sales");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecordId",
                table: "sales",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ArtistName",
                table: "sales",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecordTitle",
                table: "sales",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_vinyl_records_RecordId",
                table: "sales",
                column: "RecordId",
                principalTable: "vinyl_records",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_vinyl_records_RecordId",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "ArtistName",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "RecordTitle",
                table: "sales");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecordId",
                table: "sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_vinyl_records_RecordId",
                table: "sales",
                column: "RecordId",
                principalTable: "vinyl_records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
