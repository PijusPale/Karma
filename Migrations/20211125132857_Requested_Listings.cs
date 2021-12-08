using Microsoft.EntityFrameworkCore.Migrations;

namespace Karma.Migrations
{
    public partial class Requested_Listings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingUser",
                columns: table => new
                {
                    RequestedListingsId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequesteesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingUser", x => new { x.RequestedListingsId, x.RequesteesId });
                    table.ForeignKey(
                        name: "FK_ListingUser_Listings_RequestedListingsId",
                        column: x => x.RequestedListingsId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingUser_Users_RequesteesId",
                        column: x => x.RequesteesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingUser_RequesteesId",
                table: "ListingUser",
                column: "RequesteesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingUser");
        }
    }
}
