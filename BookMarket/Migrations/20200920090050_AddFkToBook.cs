using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class AddFkToBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {



            migrationBuilder.AddForeignKey(
                name: "FK_Book_GenreBooks_IdCategoryNavigationId",
                table: "Book",
                column: "IdCategory",
                principalTable: "GenreBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {




            migrationBuilder.AddForeignKey(
                name: "FK_Book_GenreBook",
                table: "Book",
                column: "IdCategory",
                principalTable: "GenreBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
