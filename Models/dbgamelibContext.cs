using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GameLibWeb
{
    public partial class dbgamelibContext : DbContext
    {
        public dbgamelibContext()
        {
        }

        public dbgamelibContext(DbContextOptions<dbgamelibContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Developer> Developers { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Gamegenrerelation> Gamegenrerelations { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Librarymedium> Librarymedia { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=dbgamelib;user=root;password=10203040506MySQL", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8mb3");

            modelBuilder.Entity<Developer>(entity =>
            {
                entity.ToTable("developers");

                entity.HasIndex(e => e.LibraryMediaId, "libraryMediaId_id_idx")
                    .IsUnique();

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.LibraryMedia)
                    .WithOne(p => p.Developer)
                    .HasForeignKey<Developer>(d => d.LibraryMediaId)
                    .HasConstraintName("LibraryMediaId_D_id");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("games");

                entity.HasIndex(e => e.DeveloperId, "developerId_id_idx")
                    .IsUnique();

                entity.HasIndex(e => e.LibraryMediaId, "libraryMediaId_id_idx")
                    .IsUnique();

                entity.HasIndex(e => e.PublisherId, "publisherId_idx")
                    .IsUnique();

                entity.HasIndex(e => e.RatingId, "ratingId_id_idx")
                    .IsUnique();

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Developer)
                    .WithOne(p => p.Game)
                    .HasForeignKey<Game>(d => d.DeveloperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DeveloperId_id");

                entity.HasOne(d => d.LibraryMedia)
                    .WithOne(p => p.Game)
                    .HasForeignKey<Game>(d => d.LibraryMediaId)
                    .HasConstraintName("LibraryMediaId_G_id");

                entity.HasOne(d => d.Publisher)
                    .WithOne(p => p.Game)
                    .HasForeignKey<Game>(d => d.PublisherId)
                    .HasConstraintName("PublisherId_id");

                entity.HasOne(d => d.Rating)
                    .WithOne(p => p.Game)
                    .HasForeignKey<Game>(d => d.RatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RatingId_id");
            });

            modelBuilder.Entity<Gamegenrerelation>(entity =>
            {
                entity.ToTable("gamegenrerelations");

                entity.HasIndex(e => e.GameId, "gameId_id_idx");

                entity.HasIndex(e => e.GenreId, "genreId_id_idx");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Gamegenrerelations)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GameId_G_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Gamegenrerelations)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GenreId_id");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genres");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Librarymedium>(entity =>
            {
                entity.ToTable("librarymedia");

                entity.Property(e => e.Media).HasColumnType("text");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("publishers");

                entity.HasIndex(e => e.LibraryMediaId, "libraryMediaId_id_idx")
                    .IsUnique();

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.LibraryMedia)
                    .WithOne(p => p.Publisher)
                    .HasForeignKey<Publisher>(d => d.LibraryMediaId)
                    .HasConstraintName("LibraryMediaId_P_id");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("ratings");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
