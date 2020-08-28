using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations
{
    public partial class d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Author",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 50, nullable: false),
            //        Family = table.Column<string>(maxLength: 50, nullable: false),
            //        Surname = table.Column<string>(maxLength: 50, nullable: true),
            //        DateBirth = table.Column<DateTime>(type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Author", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CategoryBook",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CategoryBook", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Book",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 100, nullable: false),
            //        Description = table.Column<string>(type: "text", nullable: true),
            //        ContentBook = table.Column<byte[]>(nullable: true),
            //        IdCategory = table.Column<int>(nullable: true),
            //        IdAuthor = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Book", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Book_Author",
            //            column: x => x.IdAuthor,
            //            principalTable: "Author",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Book_CategoryBook",
            //            column: x => x.IdCategory,
            //            principalTable: "CategoryBook",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Book_IdAuthor",
            //    table: "Book",
            //    column: "IdAuthor");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Book_IdCategory",
            //    table: "Book",
            //    column: "IdCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Book");

            //migrationBuilder.DropTable(
            //    name: "Author");

            //migrationBuilder.DropTable(
            //    name: "CategoryBook");
        }
    }
}
