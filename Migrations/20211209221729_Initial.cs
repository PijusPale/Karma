using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karma.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    AvatarPath = table.Column<string>(type: "TEXT", nullable: true),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    recipientId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationJson = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    DatePublished = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Condition = table.Column<int>(type: "INTEGER", nullable: false),
                    GardenX = table.Column<int>(type: "INTEGER", nullable: false),
                    GardenZ = table.Column<int>(type: "INTEGER", nullable: false),
                    GardenPlant = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserOneId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserTwoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ListingId = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.UniqueConstraint("AK_Conversations_GroupId", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Conversations_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversations_Users_UserOneId",
                        column: x => x.UserOneId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversations_Users_UserTwoId",
                        column: x => x.UserTwoId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    FromId = table.Column<int>(type: "INTEGER", nullable: false),
                    FromUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    GroupId = table.Column<string>(type: "TEXT", nullable: false),
                    ConversationId = table.Column<int>(type: "INTEGER", nullable: true),
                    DateSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarPath", "Email", "FirstName", "LastActive", "LastName", "Password", "Username" },
                values: new object[] { 1, null, "first@email.com", "First", null, "Test", "First", "First" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarPath", "Email", "FirstName", "LastActive", "LastName", "Password", "Username" },
                values: new object[] { 2, null, "second@email.com", "Second", null, "Test", "Second", "Second" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarPath", "Email", "FirstName", "LastActive", "LastName", "Password", "Username" },
                values: new object[] { 3, null, "third@email.com", "John", null, "Smith", "Third", "Third" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarPath", "Email", "FirstName", "LastActive", "LastName", "Password", "Username" },
                values: new object[] { 4, null, "fourth@email.com", "Anna", null, "Smith", "Fourth", "Fourth" });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "Id", "Category", "Condition", "DatePublished", "Description", "GardenPlant", "GardenX", "GardenZ", "ImagePath", "LocationJson", "Name", "Quantity", "Status", "UserId", "recipientId" },
                values: new object[] { 1, "Vehicles", 0, new DateTime(2021, 12, 1, 16, 27, 12, 258, DateTimeKind.Unspecified).AddTicks(7492), "", null, 0, 0, "images/default.png", "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\\u0160iauliai\",\"RadiusKM\":5}", "First Listing", 1, 1, 1, 2 });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "Id", "Category", "Condition", "DatePublished", "Description", "GardenPlant", "GardenX", "GardenZ", "ImagePath", "LocationJson", "Name", "Quantity", "Status", "UserId", "recipientId" },
                values: new object[] { 2, "Vehicles", 0, new DateTime(2021, 12, 2, 13, 30, 36, 970, DateTimeKind.Unspecified).AddTicks(8905), "", null, 0, 0, "images/default.png", "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\\u0160iauliai\",\"RadiusKM\":5}", "Second Listing", 1, 0, 3, null });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "Id", "Category", "Condition", "DatePublished", "Description", "GardenPlant", "GardenX", "GardenZ", "ImagePath", "LocationJson", "Name", "Quantity", "Status", "UserId", "recipientId" },
                values: new object[] { 3, "Vehicles", 0, new DateTime(2021, 12, 2, 13, 30, 43, 459, DateTimeKind.Unspecified).AddTicks(9796), "", null, 0, 0, "images/default.png", "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\\u0160iauliai\",\"RadiusKM\":5}", "Third Listing", 1, 1, 4, 1 });

            migrationBuilder.InsertData(
                table: "Conversations",
                columns: new[] { "Id", "GroupId", "ListingId", "UserOneId", "UserTwoId" },
                values: new object[] { 1, "3e888732f3a04974b3679967f92e1aff", 1, 1, 2 });

            migrationBuilder.InsertData(
                table: "Conversations",
                columns: new[] { "Id", "GroupId", "ListingId", "UserOneId", "UserTwoId" },
                values: new object[] { 2, "2b33bd58fe314cf694f848a593396208", 3, 4, 1 });

            migrationBuilder.InsertData(
                table: "ListingUser",
                columns: new[] { "RequestedListingsId", "RequesteesId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "ListingUser",
                columns: new[] { "RequestedListingsId", "RequesteesId" },
                values: new object[] { 3, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ListingId",
                table: "Conversations",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserOneId",
                table: "Conversations",
                column: "UserOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserTwoId",
                table: "Conversations",
                column: "UserTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserId",
                table: "Listings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingUser_RequesteesId",
                table: "ListingUser",
                column: "RequesteesId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages",
                column: "FromUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingUser");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
