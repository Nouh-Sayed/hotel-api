using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<HotelAmenity> HotelAmenities => Set<HotelAmenity>();
        public DbSet<HotelImage> HotelImages => Set<HotelImage>();
        public DbSet<PhotoSource> PhotoSources => Set<PhotoSource>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PricingRule> PricingRules => Set<PricingRule>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Hotel>()
                .HasIndex(h => h.ExternalId)
                .IsUnique();

            modelBuilder.Entity<Hotel>()
                .Property(h => h.PricePerNightTry)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Amenity>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<HotelAmenity>()
                .HasKey(x => new { x.HotelId, x.AmenityId });

            modelBuilder.Entity<HotelAmenity>()
                .HasOne(x => x.Hotel)
                .WithMany(h => h.HotelAmenities)
                .HasForeignKey(x => x.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HotelAmenity>()
                .HasOne(x => x.Amenity)
                .WithMany(a => a.HotelAmenities)
                .HasForeignKey(x => x.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HotelImage>()
                .HasOne(x => x.Hotel)
                .WithMany(h => h.Images)
                .HasForeignKey(x => x.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhotoSource>()
                .HasOne(x => x.Hotel)
                .WithOne(h => h.PhotoSource)
                .HasForeignKey<PhotoSource>(x => x.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasIndex(r => new { r.HotelId, r.RoomNumber })
                .IsUnique();

            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hotel)
                .WithMany(h => h.Bookings)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany()
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PricingRule>()
                .Property(p => p.Multiplier)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PricingRule>()
                .HasOne(p => p.Hotel)
                .WithMany()
                .HasForeignKey(p => p.HotelId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}