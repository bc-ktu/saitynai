using api.Data.Entities;
using api.Models;

namespace api.Data.DTOs
{
    // prideti ID
    public class CreateOrderDto
    {
       // public List<int> ProductsId { get; set; }
        public string Orderer { get; set; }

    }
    public class UpdateOrderDto
    {
        public OrderStatuses Status { get; set; }

    }
    public class OrderDto
    {
        public OrderStatuses Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEditted { get; set; }
        public string Orderer { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; } 
    }
}
