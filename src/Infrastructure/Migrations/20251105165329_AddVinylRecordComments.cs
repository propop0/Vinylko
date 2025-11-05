using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVinylRecordComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vinyl_record_comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VinylRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vinyl_record_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vinyl_record_comments_vinyl_records_VinylRecordId",
                        column: x => x.VinylRecordId,
                        principalTable: "vinyl_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_record_comments_CreatedAt",
                table: "vinyl_record_comments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_record_comments_VinylRecordId",
                table: "vinyl_record_comments",
                column: "VinylRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vinyl_record_comments");
        }
    }
}
