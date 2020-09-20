using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class AddCategoryGenreEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBook",
                table: "CategoryBook");

            migrationBuilder.RenameTable(
                name: "CategoryBook",
                newName: "GenreBooks");

            migrationBuilder.AddColumn<int>(
                name: "IdGenreCategory",
                table: "GenreBooks",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenreBooks",
                table: "GenreBooks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GenreCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreBooks_IdGenreCategory",
                table: "GenreBooks",
                column: "IdGenreCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryGenres",
                table: "GenreBooks",
                column: "IdGenreCategory",
                principalTable: "GenreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryGenres",
                table: "GenreBooks");

            migrationBuilder.DropTable(
                name: "GenreCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenreBooks",
                table: "GenreBooks");

            migrationBuilder.DropIndex(
                name: "IX_GenreBooks_IdGenreCategory",
                table: "GenreBooks");

            migrationBuilder.DropColumn(
                name: "IdGenreCategory",
                table: "GenreBooks");

            migrationBuilder.RenameTable(
                name: "GenreBooks",
                newName: "CategoryBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBook",
                table: "CategoryBook",
                column: "Id");
        }
    }
}
