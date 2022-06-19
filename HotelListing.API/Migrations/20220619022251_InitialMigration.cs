using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HotelListing.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    short_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hotels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    rating = table.Column<double>(type: "double precision", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hotels", x => x.id);
                    table.ForeignKey(
                        name: "fk_hotels_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "id", "name", "short_name" },
                values: new object[,]
                {
                    { 1, "USA", "US" },
                    { 2, "Canada", "CA" },
                    { 3, "Mexico", "MX" }
                });

            migrationBuilder.InsertData(
                table: "hotels",
                columns: new[] { "id", "address", "country_id", "name", "rating" },
                values: new object[,]
                {
                    { 1, "Geroge town", 1, "Hilton", 4.5 },
                    { 2, "Manhattan", 3, "Sheraton", 4.0 },
                    { 3, "Crazy zone", 2, "Hyatt", 3.5 },
                    { 4, "Match Match street", 1, "Marriott", 3.0 },
                    { 5, "Agege", 3, "Four Seasons", 4.0 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_hotels_country_id",
                table: "hotels",
                column: "country_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hotels");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
