using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;

namespace TrybeHotel.Repository;
public class TrybeHotelContext : DbContext, ITrybeHotelContext
{
    public DbSet<City>? Cities { get; set; }
    public DbSet<Hotel>? Hotels { get; set; }
    public DbSet<Room>? Rooms { get; set; }
    public DbSet<User>? Users { get; set; } 
    public DbSet<Booking>? Bookings { get; set; }
    public TrybeHotelContext(DbContextOptions<TrybeHotelContext> options) : base(options)
    {
        Seeder.SeedUserAdmin(this);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Server=localhost;Database=TrybeHotel;User=SA;Password=TrybeHotel12!;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().ToTable("City");
        modelBuilder.Entity<Hotel>().ToTable("Hotel");
        modelBuilder.Entity<Room>().ToTable("Room");

        modelBuilder.Entity<City>()
                    .HasMany(c => c.Hotels)
                    .WithOne(h => h.City)
                    .HasForeignKey(h => h.CityId);

        modelBuilder.Entity<Hotel>()
                    .HasMany(h => h.Rooms)
                    .WithOne(r => r.Hotel)
                    .HasForeignKey(r => r.HotelId);

        modelBuilder.Entity<Room>()
                    .HasOne(r => r.Hotel)
                    .WithMany(h => h.Rooms)
                    .HasForeignKey(r => r.RoomId);

    }
}