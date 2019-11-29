using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tietokantakirjasto
{
    public partial class HauMauPicsContext : DbContext
    {
        public HauMauPicsContext()
        {
        }

        public HauMauPicsContext(DbContextOptions<HauMauPicsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kuvakirjasto> Kuvakirjasto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connection = (string)Environment.GetEnvironmentVariable("picsconnectionString");
                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kuvakirjasto>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("kuvakirjasto");

                entity.Property(e => e.Kuva).IsRequired();

                entity.Property(e => e.ElainId).HasColumnName("elainId");

                entity.Property(e => e.KuvaId)
                    .HasColumnName("kuvaId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.KuvaNimi)
                    .IsRequired()
                    .HasColumnName("kuvaNimi")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
