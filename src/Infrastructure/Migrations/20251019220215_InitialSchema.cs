using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Website = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vinyl_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", nullable: false),
                    Genre = table.Column<string>(type: "varchar(100)", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    LabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vinyl_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vinyl_records_artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleNumber = table.Column<string>(type: "varchar(50)", nullable: false),
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CustomerName = table.Column<string>(type: "varchar(200)", nullable: true),
                    CustomerEmail = table.Column<string>(type: "varchar(300)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sales_vinyl_records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "vinyl_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artists_Country",
                table: "artists",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_artists_Name",
                table: "artists",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_genres_Name",
                table: "genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sales_CustomerEmail",
                table: "sales",
                column: "CustomerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_sales_RecordId",
                table: "sales",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_sales_SaleDate",
                table: "sales",
                column: "SaleDate");

            migrationBuilder.CreateIndex(
                name: "IX_sales_SaleNumber",
                table: "sales",
                column: "SaleNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sales_Status",
                table: "sales",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_ArtistId",
                table: "vinyl_records",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_Genre",
                table: "vinyl_records",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_LabelId",
                table: "vinyl_records",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_ReleaseYear",
                table: "vinyl_records",
                column: "ReleaseYear");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_Status",
                table: "vinyl_records",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_Title",
                table: "vinyl_records",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_vinyl_records_Title_ArtistId",
                table: "vinyl_records",
                columns: new[] { "Title", "ArtistId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "sales");

            migrationBuilder.DropTable(
                name: "vinyl_records");

            migrationBuilder.DropTable(
                name: "artists");
        }
    }
}
