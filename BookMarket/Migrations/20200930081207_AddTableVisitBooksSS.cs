using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class AddTableVisitBooksSS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_visitUser",
                table: "visitUser");

            migrationBuilder.RenameTable(
                name: "visitUser",
                newName: "UsersVisit");

            migrationBuilder.RenameIndex(
                name: "IX_visitUser_IdBook",
                table: "UsersVisit",
                newName: "IX_UsersVisit_IdBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersVisit",
                table: "UsersVisit",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersVisit",
                table: "UsersVisit");

            migrationBuilder.RenameTable(
                name: "UsersVisit",
                newName: "visitUser");

            migrationBuilder.RenameIndex(
                name: "IX_UsersVisit_IdBook",
                table: "visitUser",
                newName: "IX_visitUser_IdBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_visitUser",
                table: "visitUser",
                column: "Id");
        }
    }
}
