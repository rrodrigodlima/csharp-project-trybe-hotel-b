using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;

namespace TrybeHotel.Repository;
public class TrybeHotelContext : DbContext, ITrybeHotelContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; } // Adicione esta linha
    public DbSet<Booking> Bookings { get; set; }
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
    // Configuração do mapeamento para a entidade City
    modelBuilder.Entity<City>(entity =>
    {
        entity.HasKey(c => c.CityId); // Definindo a chave primária
        entity.Property(c => c.Name).IsRequired();
    });

    // Configuração do mapeamento para a entidade Hotel
    modelBuilder.Entity<Hotel>(entity =>
    {
        entity.HasKey(h => h.HotelId); // Definindo a chave primária
        entity.Property(h => h.Name).IsRequired();
        entity.Property(h => h.Address).IsRequired();
        entity.Property(h => h.CityId).IsRequired();
        
        // Definindo o relacionamento com City
        entity.HasOne(h => h.City)
            .WithMany()
            .HasForeignKey(h => h.CityId)
            .OnDelete(DeleteBehavior.Restrict); 
    });

    // Configuração do mapeamento para a entidade Room
    modelBuilder.Entity<Room>(entity =>
    {
        entity.HasKey(r => r.RoomId); // Definindo a chave primária
        entity.Property(r => r.Name).IsRequired();
        entity.Property(r => r.Capacity).IsRequired();
        entity.Property(r => r.Image).IsRequired();
        entity.Property(r => r.HotelId).IsRequired();
        
        // Definindo o relacionamento com Hotel
        entity.HasOne(r => r.Hotel)
            .WithMany()
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict); 
    });

    // Configuração do mapeamento para a entidade User
    modelBuilder.Entity<User>(entity =>
    {
        entity.HasKey(u => u.UserId); // Definindo a chave primária
        entity.Property(u => u.Name).IsRequired();
        entity.Property(u => u.Email).IsRequired();
        entity.Property(u => u.Password).IsRequired();
        entity.Property(u => u.UserType).IsRequired();
        
        // Definindo o relacionamento com Bookings
        entity.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
    });

    // Configuração do mapeamento para a entidade Booking
    modelBuilder.Entity<Booking>(entity =>
    {
        entity.HasKey(b => b.BookingId); // Definindo a chave primária
        entity.Property(b => b.CheckIn).IsRequired();
        entity.Property(b => b.CheckOut).IsRequired();
        entity.Property(b => b.GuestQuant).IsRequired();
        
        // Definindo os relacionamentos com User e Room
        entity.HasOne(b => b.User)
            .WithMany(u => u.Bookings)  
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict); 

        entity.HasOne(b => b.Room)
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict); 
    });
}
}