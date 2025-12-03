using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistGenresAndRecordReleaseTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "artist_genres",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    GenreId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_genres", x => new { x.ArtistId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_artist_genres_artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artist_genres_genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "record_release_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VinylRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_record_release_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_record_release_types_vinyl_records_VinylRecordId",
                        column: x => x.VinylRecordId,
                        principalTable: "vinyl_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artist_genres_ArtistId",
                table: "artist_genres",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_artist_genres_GenreId",
                table: "artist_genres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_record_release_types_Type",
                table: "record_release_types",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_record_release_types_VinylRecordId",
                table: "record_release_types",
                column: "VinylRecordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artist_genres");

            migrationBuilder.DropTable(
                name: "record_release_types");
        }
    }
}
