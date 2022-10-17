using Microsoft.EntityFrameworkCore.Migrations;

namespace Airlines25554.Migrations
{
    public partial class AddPurchasedTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PurchasedTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FlightId = table.Column<int>(type: "int", nullable: true),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasedTickets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedTickets_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_FlightId",
                table: "PurchasedTickets",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_UserId",
                table: "PurchasedTickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Tickets");
        }
    }
}
