using System;
using System.Collections.Generic;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Context
{
    public partial class DbDataContext : DbContext
    {
        public DbDataContext()
        {
        }

        public DbDataContext(DbContextOptions<DbDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Insurance> Insurances { get; set; } = null!;
        public virtual DbSet<InsuranceInsured> InsuranceInsureds { get; set; } = null!;
        public virtual DbSet<Insured> Insureds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-ACPBVCU\\SQLEXPRESS;Database=DB_Seguros;Encrypt=True;TrustServerCertificate=True;User Id=sa;Password=jona;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.ToTable("Insurance");

                entity.Property(e => e.InsuranceCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuredAmount).HasColumnType("decimal(12, 6)");

                entity.Property(e => e.Prima).HasColumnType("decimal(12, 6)");
            });

            modelBuilder.Entity<InsuranceInsured>(entity =>
            {
                entity.ToTable("InsuranceInsured");

                entity.Property(e => e.IdInsurance).HasColumnName("Id_Insurance");

                entity.Property(e => e.IdInsured).HasColumnName("Id_Insured");

                entity.HasOne(d => d.IdInsuranceNavigation)
                    .WithMany(p => p.InsuranceInsureds)
                    .HasForeignKey(d => d.IdInsurance)
                    .HasConstraintName("FK__Insurance__Id_In__3B75D760");

                entity.HasOne(d => d.IdInsuredNavigation)
                    .WithMany(p => p.InsuranceInsureds)
                    .HasForeignKey(d => d.IdInsured)
                    .HasConstraintName("FK__Insurance__Id_In__3C69FB99");
            });

            modelBuilder.Entity<Insured>(entity =>
            {
                entity.ToTable("Insured");

                entity.Property(e => e.Identification)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.InsuredName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
