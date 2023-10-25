namespace TrybeHotel.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // 1. Implemente as models da aplicação
    public class City
    {
        public int CityId { get; set; } // Chave primária
        public string Name { get; set; } = string.Empty;
        // Propriedade de navegação para os hotéis
        public ICollection<Hotel>? Hotels { get; set; }
    }
}