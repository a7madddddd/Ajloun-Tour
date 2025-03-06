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
        public virtual DbSet<BookingOption> BookingOptions { get; set; } = null!;
        public virtual DbSet<BookingOptionSelection> BookingOptionSelections { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<ContactMessage> ContactMessages { get; set; } = null!;
        public virtual DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; } = null!;
        public virtual DbSet<Offer> Offers { get; set; } = null!;
        public virtual DbSet<OfferProgram> OfferPrograms { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<PackageProgram> PackagePrograms { get; set; } = null!;
        public virtual DbSet<Program> Programs { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Testomonial> Testomonials { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;
        public virtual DbSet<TourOffer> TourOffers { get; set; } = null!;
        public virtual DbSet<TourPackage> TourPackages { get; set; } = null!;
        public virtual DbSet<TourProgram> TourPrograms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

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
                entity.HasIndex(e => e.CartId, "IX_CartItems_CartID");

                entity.HasIndex(e => e.TourId, "IX_CartItems_TourID");

                entity.Property(e => e.CartItemId).HasColumnName("CartItemID");

                entity.Property(e => e.BikeRentPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DinnerPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HasBikeRent).HasDefaultValueSql("((0))");

                entity.Property(e => e.HasDinner).HasDefaultValueSql("((0))");

                entity.Property(e => e.HasInsurance).HasDefaultValueSql("((0))");

                entity.Property(e => e.HasTourGuide).HasDefaultValueSql("((0))");

                entity.Property(e => e.InsurancePrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.SelectedDate).HasColumnType("date");

                entity.Property(e => e.TourGuidePrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CartItems__CartI__3587F3E0");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CartItems__TourI__367C1819");
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
