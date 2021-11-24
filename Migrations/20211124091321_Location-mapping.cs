using Microsoft.EntityFrameworkCore.Migrations;

namespace Karma.Migrations
{
    public partial class Locationmapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationJson",
                table: "Listings",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationJson",
                table: "Listings");
        }
    }
}
