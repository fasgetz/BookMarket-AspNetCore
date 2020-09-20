using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class AddFkToBook2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_GenreBooks_IdCategoryNavigationId",
                table: "Book");


            migrationBuilder.AddForeignKey(
                name: "FK_Book_GenreBook",
                table: "Book",
                column: "IdCategory",
                principalTable: "GenreBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_GenreBook",
                table: "Book");



            migrationBuilder.AddForeignKey(
                name: "FK_Book_GenreBooks_IdCategoryNavigationId",
                table: "Book",
                column: "IdCategoryNavigationId",
                principalTable: "GenreBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
