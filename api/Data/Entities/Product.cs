
using api.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Kaina turi būti didesnė nei 0")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Privalomas laukas")]
        [MaxLength(30, ErrorMessage = "Šis laukas turi nuo 3 iki 30 simbolių")]
        [MinLength(3, ErrorMessage = "Šis laukas turi nuo 3 iki 30 simbolių")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Privalomas laukas")]
        [MaxLength(30, ErrorMessage = "Šis laukas turi nuo 3 iki 30 simbolių")]
        [MinLength(3, ErrorMessage = "Šis laukas turi nuo 3 iki 30 simbolių")]
        public string Type { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Privalomas laukas")]
        [MaxLength(500, ErrorMessage = "Šis laukas turi nuo 3 iki 500 simbolių")]
        [MinLength(3, ErrorMessage = "Šis laukas turi nuo 3 iki 500 simbolių")]
        public string Description { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue, ErrorMessage = "Kiekis negali būti neigiamas")]
        public int Quantity { get; set; } = 1;
        public bool CanBeBought { get; set; } = false;
        public bool IsDisplayed { get; set; } = false;
        public string Creator { get; set; } // keisti į ID
                                            // who created a product, if Product.Creator == Order.Orderer and Order.Status = Pateiktas then a user can delete the product
        public Order? Order { get; set; } = null;
        public int? OrderId { get; set; } = null;
        public List<Comment>? Comments { get; set; } = null;
    }
}