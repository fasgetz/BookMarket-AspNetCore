using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class addNewEntityFavoriteUserBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "FavoriteUserBook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "FavoriteUserBook",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BookFavoriteId = table.Column<int>(type: "int", nullable: true),
            //        IdBookFavorite = table.Column<int>(type: "int", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FavoriteUserBook", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_FavoriteUserBook_Book_BookFavoriteId",
            //            column: x => x.BookFavoriteId,
            //            principalTable: "Book",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteUserBook_BookFavoriteId",
            //    table: "FavoriteUserBook",
            //    column: "BookFavoriteId");
        }
    }
}
