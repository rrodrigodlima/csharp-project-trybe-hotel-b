namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Room
{
  public int RoomId { get; set; } // Chave primária
  public string Name { get; set; } = string.Empty;
  public int Capacity { get; set; }
  public string Image { get; set; } = string.Empty;
  public int HotelId { get; set; } // Chave estrangeira para a model Hotel

  // Propriedade de navegação para o hotel
  public Hotel? Hotel { get; set; }
  public ICollection<Booking>? Bookings { get; set; }
}