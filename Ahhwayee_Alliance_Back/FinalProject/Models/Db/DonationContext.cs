using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FinalProject.Models.Db
{
    public partial class DonationContext : DbContext
    {
        public DonationContext()
        {
        }

        public DonationContext(DbContextOptions<DonationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<EvaluateCost> EvaluateCosts { get; set; }
        public virtual DbSet<ShelterInformation> ShelterInformations { get; set; }
        public virtual DbSet<TransferDonation> TransferDonations { get; set; }
        public virtual DbSet<UserImage> UserImages { get; set; }
        public virtual DbSet<UserInformation> UserInformations { get; set; }
        public virtual DbSet<VerifyForm> VerifyForms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.ToTable("Donation");

                entity.Property(e => e.DonationId).ValueGeneratedNever();

                entity.Property(e => e.DonationTime).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<EvaluateCost>(entity =>
            {
                entity.HasKey(e => new { e.ShelterId, e.PurposeId })
                    .HasName("PK_EvaluateCosts");

                entity.ToTable("EvaluateCost");
            });

            modelBuilder.Entity<ShelterInformation>(entity =>
            {
                entity.HasKey(e => e.ShelterId);

                entity.ToTable("ShelterInformation");

                entity.Property(e => e.ShelterId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength(true);

                entity.Property(e => e.ShelterImgName).HasMaxLength(10);

                entity.Property(e => e.ShelterImgUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShelterName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ShelterPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<TransferDonation>(entity =>
            {
                entity.HasKey(e => new { e.ShelterId, e.TransferDate, e.PurposeId });

                entity.ToTable("TransferDonation");

                entity.Property(e => e.TransferDate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<UserImage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UserImage");

                entity.Property(e => e.Image).HasColumnType("image");
            });

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserInformation_1");

                entity.ToTable("UserInformation");

                entity.HasIndex(e => e.Account, "IX_UserInformation")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserImage).HasColumnType("image");

                entity.Property(e => e.UserImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ValidToken)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VerifyForm>(entity =>
            {
                entity.HasKey(e => e.Account);

                entity.ToTable("VerifyForm");

                entity.Property(e => e.Account)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VerifyCode)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
