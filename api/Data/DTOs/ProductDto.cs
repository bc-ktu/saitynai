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
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        public string Creator { get; set; } // change dto

    }
    //, List<CommentDto> Comments);
    public class UpdateProductQuantityDto
    {
        public int Quantity { get; set; }
    }
}