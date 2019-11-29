using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tietokantakirjasto
{
    public partial class RyhmahaumauContext : DbContext
    {
        public RyhmahaumauContext()
        {
        }

        public RyhmahaumauContext(DbContextOptions<RyhmahaumauContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Elain> Elain { get; set; }
        public virtual DbSet<Kayttaja> Kayttaja { get; set; }
        public virtual DbSet<Laji> Laji { get; set; }
        public virtual DbSet<Kuva> Kuva { get; set; }
        public virtual DbSet<Yllapito> Yllapito { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connection = (string)Environment.GetEnvironmentVariable("connectionString");
                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elain>(entity =>
            {
                entity.ToTable("elain");

                entity.Property(e => e.ElainId).HasColumnName("elain_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Kuvaus)
                    .HasColumnName("kuvaus")
                    .HasMaxLength(255);

                entity.Property(e => e.LajiId).HasColumnName("laji_id");

                entity.Property(e => e.Nimi)
                    .IsRequired()
                    .HasColumnName("nimi")
                    .HasMaxLength(50);

                entity.Property(e => e.Rotu)
                    .HasColumnName("rotu")
                    .HasMaxLength(50);

                entity.Property(e => e.Syntymapaiva)
                    .HasColumnName("syntymapaiva")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.Elain)
                    .HasForeignKey(d => d.KayttajaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_elain_kayttaja");

                entity.HasOne(d => d.Laji)
                    .WithMany(p => p.Elain)
                    .HasForeignKey(d => d.LajiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_elain_laji");
            });

            modelBuilder.Entity<Kayttaja>(entity =>
            {
                entity.ToTable("kayttaja");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Sahkoposti).HasColumnName("sahkoposti");

                entity.Property(e => e.Postinumero)
                    .IsRequired()
                    .HasColumnName("postinumero")
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<Laji>(entity =>
            {
                entity.ToTable("laji");

                entity.Property(e => e.LajiId).HasColumnName("laji_id");

                entity.Property(e => e.Nimi)
                    .IsRequired()
                    .HasColumnName("nimi")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Kuva>(entity =>
            {
                entity.Property(e => e.KuvaId).HasColumnName("KuvaID");

                entity.Property(e => e.IsoKuvaFileName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ThumbnailKuvaFileName).HasMaxLength(50);
            });

            modelBuilder.Entity<Yllapito>(entity =>
            {
                entity.HasKey(e => e.ViestiId)
                    .HasName("PK__yllapito__1D27EFD12A83D7C1");

                entity.ToTable("yllapito");

                entity.Property(e => e.ViestiId).HasColumnName("viesti_id");

                entity.Property(e => e.Otsikko)
                    .IsRequired()
                    .HasColumnName("otsikko")
                    .HasMaxLength(60);

                entity.Property(e => e.Pvm)
                    .HasColumnName("pvm")
                    .HasColumnType("datetime");

                entity.Property(e => e.Viesti)
                    .IsRequired()
                    .HasColumnName("viesti")
                    .HasMaxLength(300);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
