using Microsoft.EntityFrameworkCore.Migrations;

namespace Airlines25554.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Countries_CountryId",
                table: "Airports");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Airports",
                newName: "AirportId");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_CountryId",
                table: "Airports",
                newName: "IX_Airports_AirportId");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Airports_AirportId",
                table: "Airports",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Airports_AirportId",
                table: "Airports");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "AirportId",
                table: "Airports",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_AirportId",
                table: "Airports",
                newName: "IX_Airports_CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Countries_CountryId",
                table: "Airports",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
