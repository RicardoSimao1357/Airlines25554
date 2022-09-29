using Microsoft.EntityFrameworkCore.Migrations;

namespace Airlines25554.Migrations
{
    public partial class teste2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Airports_AirportId",
                table: "Airports");

            migrationBuilder.DropIndex(
                name: "IX_Airports_AirportId",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "AirportId",
                table: "Airports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirportId",
                table: "Airports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_AirportId",
                table: "Airports",
                column: "AirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Airports_AirportId",
                table: "Airports",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
