using api.Data.Entities;
using api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public OrderStatuses Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEditted { get; set; }
        public string Orderer { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; } //without shipping
        public List<Product> Products { get; set; }
    }
}