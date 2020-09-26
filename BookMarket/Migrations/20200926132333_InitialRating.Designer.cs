﻿// <auto-generated />
using System;
using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookMarket.Migrations
{
    [DbContext(typeof(BookMarketContext))]
    [Migration("20200926132333_InitialRating")]
    partial class InitialRating
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookMarket.Models.DataBase.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateBirth")
                        .HasColumnType("date");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddDatabase")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("IdAuthor")
                        .HasColumnType("int");

                    b.Property<int?>("IdCategory")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<byte[]>("PosterBook")
                        .HasColumnType("image");

                    b.HasKey("Id");

                    b.HasIndex("IdAuthor");

                    b.HasIndex("IdCategory");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.CategoryGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GenreCategory");
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.ChapterBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChapterContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChapterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<int?>("IdBook")
                        .HasColumnType("int");

                    b.Property<int>("NumberChapter")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdBook");

                    b.ToTable("ChapterBook");
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.GenreBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IdGenreCategory")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("IdGenreCategory");

                    b.ToTable("GenreBooks");
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdBook")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Mark")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("IdBook");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.Book", b =>
                {
                    b.HasOne("BookMarket.Models.DataBase.Author", "IdAuthorNavigation")
                        .WithMany("Book")
                        .HasForeignKey("IdAuthor")
                        .HasConstraintName("FK_Book_Author")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookMarket.Models.DataBase.GenreBook", "IdCategoryNavigation")
                        .WithMany("Book")
                        .HasForeignKey("IdCategory")
                        .HasConstraintName("FK_Book_GenreBook")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.ChapterBook", b =>
                {
                    b.HasOne("BookMarket.Models.DataBase.Book", "IdBookNavigation")
                        .WithMany("ChapterBook")
                        .HasForeignKey("IdBook")
                        .HasConstraintName("FK_ChapterBook_Book")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.GenreBook", b =>
                {
                    b.HasOne("BookMarket.Models.DataBase.CategoryGenre", "GenreCategory")
                        .WithMany("GenresBook")
                        .HasForeignKey("IdGenreCategory")
                        .HasConstraintName("FK_CategoryGenres")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookMarket.Models.DataBase.Rating", b =>
                {
                    b.HasOne("BookMarket.Models.DataBase.Book", "BookRating")
                        .WithMany("UserRating")
                        .HasForeignKey("IdBook")
                        .HasConstraintName("FK_BookRatings")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
