using Microsoft.EntityFrameworkCore;

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

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("games");

                entity.HasIndex(e => e.DeveloperId, "developerId_id_idx");

                entity.HasIndex(e => e.PublisherId, "publisherId_idx");

                entity.HasIndex(e => e.RatingId, "ratingId_id_idx");

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Developer)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.DeveloperId)
                    .HasConstraintName("DeveloperId_id");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("PublisherId_id");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.RatingId)
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
                    .HasConstraintName("GameId_G_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Gamegenrerelations)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("GenreId_id");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genres");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("publishers");

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);
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
