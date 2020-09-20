﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookMarket.Models.DataBase
{
    #region Конфигурации

    /// <summary>
    /// Конфигурация жанра книги один ко многим
    /// </summary>
    public class GenreBookConfiguration : IEntityTypeConfiguration<GenreBook>
    {
        public void Configure(EntityTypeBuilder<GenreBook> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.GenreCategory)
                .WithMany(p => p.GenresBook)
                .HasForeignKey(d => d.IdGenreCategory)
                .OnDelete(DeleteBehavior.Cascade)                
                .HasConstraintName("FK_CategoryGenres");
        }
    }

    #endregion


    public partial class BookMarketContext : DbContext
    {
        public BookMarketContext()
        {
        }

        public BookMarketContext(DbContextOptions<BookMarketContext> options)
            : base(options)
        {

        }


        public virtual DbSet<CategoryGenre> GenreCategory { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<GenreBook> GenreBooks { get; set; }
        public virtual DbSet<ChapterBook> ChapterBook { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-4LR05H2\\SQLEXPRESS;Database=BookMarket;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.DateBirth).HasColumnType("date");

                entity.Property(e => e.Family)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PosterBook).HasColumnType("image");

                entity.HasOne(d => d.IdAuthorNavigation)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.IdAuthor)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Book_Author");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Book_GenreBook");
            });

            modelBuilder.ApplyConfiguration(new GenreBookConfiguration());

            modelBuilder.Entity<ChapterBook>(entity =>
            {
                entity.Property(e => e.ChapterName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.IdBookNavigation)
                    .WithMany(p => p.ChapterBook)
                    .HasForeignKey(d => d.IdBook)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChapterBook_Book");
            });




            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
