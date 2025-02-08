using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ajloun_Tour.Models
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<ContactMessage> ContactMessages { get; set; } = null!;
        public virtual DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-E98DOJ0;Database=AjlounTour;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534FA8AA19C")
                    .IsUnique();

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.AdminImage).HasColumnName("adminImage");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('Pending')");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TourId)
                    .HasConstraintName("FK__Bookings__TourID__412EB0B6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Bookings__UserID__4222D4EF");
            });

            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK__ContactM__C87C037C773D9E94");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Subject).HasMaxLength(200);

                entity.Property(e => e.SubmittedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<NewsletterSubscriber>(entity =>
            {
                entity.HasKey(e => e.SubscriberId)
                    .HasName("PK__Newslett__7DFEB634A2AED870");

                entity.HasIndex(e => e.Email, "UQ__Newslett__A9D10534CFBE192D")
                    .IsUnique();

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.SubscribedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.Duration).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TourName).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534703FE199")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
