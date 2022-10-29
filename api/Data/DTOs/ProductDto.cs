using api.Entities;

namespace api.DTOs
{
    // prideti ID
    public class ProductDto
    {
        public string Id { get; set; }
        public decimal? Price { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool CanBeBought { get; set; }
        public bool IsDisplayed { get; set; }

    }
    public class CreateProductDto
    {
        public decimal? Price { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool CanBeBought { get; set; }
        public int CreatorId { get; set; } 
        public bool IsDisplayed { get; set; }

    }
}