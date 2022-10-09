using api.Entities;

namespace api.DTOs
{
    public class ProductDto
    {
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
        public string Creator { get; set; } 
        public bool IsDisplayed { get; set; }

    }
}