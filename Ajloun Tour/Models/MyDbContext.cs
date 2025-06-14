﻿using System;
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
        public virtual DbSet<BookingOption> BookingOptions { get; set; } = null!;
        public virtual DbSet<BookingOptionSelection> BookingOptionSelections { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ContactMessage> ContactMessages { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<JobApplication> JobApplications { get; set; } = null!;
        public virtual DbSet<JobImage> JobImages { get; set; } = null!;
        public virtual DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; } = null!;
        public virtual DbSet<Offer> Offers { get; set; } = null!;
        public virtual DbSet<OfferProgram> OfferPrograms { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<PackageProgram> PackagePrograms { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; } = null!;
        public virtual DbSet<PaymentGateway> PaymentGateways { get; set; } = null!;
        public virtual DbSet<PaymentHistory> PaymentHistories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public virtual DbSet<Program> Programs { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Testomonial> Testomonials { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;
        public virtual DbSet<TourPackage> TourPackages { get; set; } = null!;
        public virtual DbSet<TourProgram> TourPrograms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<TourOffer> TourOffers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=A7MAD;Database=AjlounTour;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534A0BEF8DF")
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

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('Pending')");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.OfferId)
                    .HasConstraintName("FK_Bookings_Offer");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_Bookings_Package");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TourId)
                    .HasConstraintName("FK__Bookings__TourID__4AB81AF0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Bookings__UserID__4BAC3F29");
            });

            modelBuilder.Entity<BookingOption>(entity =>
            {
                entity.HasKey(e => e.OptionId)
                    .HasName("PK__BookingO__92C7A1DF7595E8AA");

                entity.Property(e => e.OptionId).HasColumnName("OptionID");

                entity.Property(e => e.OptionName).HasMaxLength(100);

                entity.Property(e => e.OptionPrice).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<BookingOptionSelection>(entity =>
            {
                entity.HasKey(e => e.SelectionId)
                    .HasName("PK__BookingO__7F17912F1A501E7A");

                entity.Property(e => e.SelectionId).HasColumnName("SelectionID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.OptionId).HasColumnName("OptionID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.BookingOptionSelections)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__BookingOp__Booki__14270015");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.BookingOptionSelections)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK__BookingOp__Optio__151B244E");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Carts_UserID");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('active')");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Carts__UserID__282DF8C2");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(e => e.CartItemId).HasColumnName("CartItemID");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.Option1)
                    .HasMaxLength(255)
                    .HasColumnName("option1");

                entity.Property(e => e.Option1Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("option1Price");

                entity.Property(e => e.Option2)
                    .HasMaxLength(255)
                    .HasColumnName("option2");

                entity.Property(e => e.Option2Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("option2Price");

                entity.Property(e => e.Option3)
                    .HasMaxLength(255)
                    .HasColumnName("option3");

                entity.Property(e => e.Option3Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("option3Price");

                entity.Property(e => e.Option4)
                    .HasMaxLength(255)
                    .HasColumnName("option4");

                entity.Property(e => e.Option4Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("option4Price");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SelectedDate).HasColumnType("date");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_CartItems_Booking");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartItems_Cart");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.OfferId)
                    .HasConstraintName("FK_CartItems_Offer");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_CartItems_Package");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CartItems_Products");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.TourId)
                    .HasConstraintName("FK_CartItems_Tour");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK__ContactM__C87C037C71194994");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Subject).HasMaxLength(200);

                entity.Property(e => e.SubmittedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Employee__A9D105343C915A16")
                    .IsUnique();

                entity.HasIndex(e => e.ApplicationId, "UQ__Employee__C93A4F78E0519C03")
                    .IsUnique();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HireDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Application)
                    .WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employees_JobApplications");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_Employees_Jobs");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Experinces).HasColumnType("ntext");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.JobType).HasMaxLength(50);

                entity.Property(e => e.Overview).HasColumnType("ntext");

                entity.Property(e => e.Requirements).HasColumnType("ntext");

                entity.Property(e => e.Salary).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .HasName("PK__JobAppli__C93A4F79E8A1C7C5");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.ApplicantName).HasMaxLength(100);

                entity.Property(e => e.ApplicationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Cvpath)
                    .HasMaxLength(255)
                    .HasColumnName("CVPath");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Message).HasColumnType("ntext");

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobApplications)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK__JobApplic__JobID__2F9A1060");
            });

            modelBuilder.Entity<JobImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__JobImage__7516F4ECA56E20F0");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobImages)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK__JobImages__JobID__336AA144");
            });

            modelBuilder.Entity<NewsletterSubscriber>(entity =>
            {
                entity.HasKey(e => e.SubscriberId)
                    .HasName("PK__Newslett__7DFEB6343D56CCE9");

                entity.HasIndex(e => e.Email, "UQ__Newslett__A9D10534589F2127")
                    .IsUnique();

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SubscribedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("offers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DiscountPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("discount_percentage");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Peapole).HasColumnName("peapole");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<OfferProgram>(entity =>
            {
                entity.HasIndex(e => e.OfferId, "IX_OfferPrograms_OfferId");

                entity.HasIndex(e => e.ProgramId, "IX_OfferPrograms_ProgramId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomTitle).HasMaxLength(100);

                entity.Property(e => e.ProgramDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.OfferPrograms)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferPrograms_Offers");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.OfferPrograms)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferPrograms_Programs");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.ToTable("packages");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.Map).HasColumnName("map");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.NumberOfPeople).HasColumnName("numberOfPeople");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");
            });

            modelBuilder.Entity<PackageProgram>(entity =>
            {
                entity.HasIndex(e => e.PackageId, "IX_PackagePrograms_PackageId");

                entity.HasIndex(e => e.ProgramId, "IX_PackagePrograms_ProgramId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomTitle).HasMaxLength(100);

                entity.Property(e => e.ProgramDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.PackagePrograms)
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PackagePrograms_Packages");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.PackagePrograms)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PackagePrograms_Programs");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GatewayId).HasColumnName("GatewayID");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TransactionID");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Payments__Bookin__1C873BEC");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK__Payments__CartId__1F63A897");

                entity.HasOne(d => d.Gateway)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.GatewayId)
                    .HasConstraintName("FK__Payments__Gatewa__1E6F845E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Payments__UserID__1D7B6025");
            });

            modelBuilder.Entity<PaymentDetail>(entity =>
            {
                entity.Property(e => e.PaymentDetailId).HasColumnName("PaymentDetailID");

                entity.Property(e => e.BillingAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BillingCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BillingCountry)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BillingZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CardHolderName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cvv)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CVV");

                entity.Property(e => e.ExpiryDate)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PaymentDetails)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__PaymentDe__Payme__22401542");
            });

            modelBuilder.Entity<PaymentGateway>(entity =>
            {
                entity.HasKey(e => e.GatewayId)
                    .HasName("PK__PaymentG__66BCD88047240090");

                entity.Property(e => e.GatewayId).HasColumnName("GatewayID");

                entity.Property(e => e.ApiKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Environment)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GatewayName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.SecretKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.WebhookUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.ToTable("PaymentHistory");

                entity.Property(e => e.PaymentHistoryId).HasColumnName("PaymentHistoryID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PaymentHistories)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__PaymentHi__Payme__2610A626");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Dimensions).HasMaxLength(100);

                entity.Property(e => e.DiscountPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Tag).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Products__Catego__50FB042B");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ProductI__7516F4ECD2A7B482");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.IsThumbnail).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductIm__Produ__54CB950F");
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.ProjectImage).HasColumnName("projectImage");

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(100)
                    .HasColumnName("projectName");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasColumnName("status");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__Projects__AdminI__5EBF139D");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasColumnType("text")
                    .HasColumnName("comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Subject)
                    .HasColumnType("text")
                    .HasColumnName("subject");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.OfferId)
                    .HasConstraintName("FK_Reviews_Offers");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_Reviews_Packages");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Reviews_Products");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__reviews__created__71D1E811");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__reviews__UserID__72C60C4A");
            });

            modelBuilder.Entity<Testomonial>(entity =>
            {
                entity.HasKey(e => e.TestomoId)
                    .HasName("PK__testomon__135984F43F86520D");

                entity.ToTable("testomonials");

                entity.Property(e => e.TestomoId).HasColumnName("testomoId");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .HasColumnName("message");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Testomonials)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__testomoni__UserI__619B8048");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.Property(e => e.TourId).HasColumnName("TourID");
                entity.Property(e => e.Duration).HasMaxLength(50);
                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.TourName).HasMaxLength(100);

                entity.HasMany(d => d.Offers)
                    .WithMany(p => p.Tours)
                    .UsingEntity<TourOffer>(
                        j =>
                        {
                            j.HasOne(to => to.Offer)
                                .WithMany()
                                .HasForeignKey(to => to.OfferId)
                                .HasConstraintName("FK__TourOffer__offer__0F624AF8");

                            j.HasOne(to => to.Tour)
                                .WithMany()
                                .HasForeignKey(to => to.TourId)
                                .HasConstraintName("FK__TourOffer__tour___0E6E26BF");

                            j.HasKey(to => new { to.TourId, to.OfferId })
                                .HasName("PK__TourOffe__7B2B8E4A3F82AF83");

                            j.ToTable("TourOffers");

                            j.Property(to => to.TourId).HasColumnName("tour_id");
                            j.Property(to => to.OfferId).HasColumnName("offer_id");
                        });

                modelBuilder.Entity<TourPackage>(entity =>
                {
                    entity.HasKey(e => new { e.TourId, e.PackageId })
                        .HasName("PK__TourPack__DD2EFF48ADCDAF59");

                    entity.Property(e => e.TourId).HasColumnName("tour_id");

                    entity.Property(e => e.PackageId).HasColumnName("package_id");

                    entity.Property(e => e.IsActive).HasColumnName("isActive");

                    entity.HasOne(d => d.Package)
                        .WithMany(p => p.TourPackages)
                        .HasForeignKey(d => d.PackageId)
                        .HasConstraintName("FK__TourPacka__packa__0B91BA14");

                    entity.HasOne(d => d.Tour)
                        .WithMany(p => p.TourPackages)
                        .HasForeignKey(d => d.TourId)
                        .HasConstraintName("FK__TourPacka__tour___0A9D95DB");
                });

            });

            modelBuilder.Entity<TourPackage>(entity =>
            {
                entity.HasKey(e => new { e.TourId, e.PackageId })
                    .HasName("PK__TourPack__DD2EFF48ADCDAF59");

                entity.Property(e => e.TourId).HasColumnName("tour_id");

                entity.Property(e => e.PackageId).HasColumnName("package_id");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.TourPackages)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__TourPacka__packa__0B91BA14");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.TourPackages)
                    .HasForeignKey(d => d.TourId)
                    .HasConstraintName("FK__TourPacka__tour___0A9D95DB");
            });

            modelBuilder.Entity<TourProgram>(entity =>
            {
                entity.HasIndex(e => e.ProgramId, "IX_TourPrograms_ProgramId");

                entity.HasIndex(e => e.TourId, "IX_TourPrograms_TourId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomTitle).HasMaxLength(100);

                entity.Property(e => e.ProgramDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.TourPrograms)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TourPrograms_Programs");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.TourPrograms)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TourPrograms_Tours");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534DCECE833")
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
