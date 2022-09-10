using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Airlines25554.Migrations
{
    public partial class ImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AirPlanes");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "AirPlanes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AirPlanes");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AirPlanes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
