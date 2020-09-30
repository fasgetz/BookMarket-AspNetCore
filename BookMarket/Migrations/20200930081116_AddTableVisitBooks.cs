using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class AddTableVisitBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "visitUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateVisit = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<string>(nullable: true),
                    IdBook = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersBookVisits",
                        column: x => x.IdBook,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_visitUser_IdBook",
                table: "visitUser",
                column: "IdBook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "visitUser");
        }
    }
}
