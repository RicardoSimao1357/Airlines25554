using Microsoft.EntityFrameworkCore.Migrations;

namespace Airlines25554.Migrations
{
    public partial class testeAirplains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutiveSeats",
                table: "AirPlanes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExecutiveSeats",
                table: "AirPlanes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
