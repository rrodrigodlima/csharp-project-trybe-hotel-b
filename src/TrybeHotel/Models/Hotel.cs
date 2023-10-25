namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Hotel
{
  public int HotelId { get; set; } // Chave primária
  public string? Name { get; set; }
  public string? Address { get; set; }
  public int CityId { get; set; } // Chave estrangeira para a model City

  // Propriedade de navegação para os quartos
  public List<Room>? Rooms { get; set; }

  // Propriedade de navegação para a cidade
  public City? City { get; set; }
}