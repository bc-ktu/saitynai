using api.Data.Entities;
using api.Models;

namespace api.Data.DTOs
{
    public class CreateOrderDto
    {
       // public List<int> ProductsId { get; set; }
        public string Orderer { get; set; }

    }
    public class UpdateOrderDto
    {
        public OrderStatuses Status { get; set; }

    }
    /*public class UpdateOrderStatusDto
    {
        public OrderStatuses Status { get; set; } = OrderStatuses.Atšauktas;
        public DateTime DateEditted { get; set; } = DateTime.Now;

    }*/

    public class OrderDto
    {
        public OrderStatuses Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEditted { get; set; }
        public string Orderer { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; } 
       // public List<Product> Products { get; set; }
    }
}
