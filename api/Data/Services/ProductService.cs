using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetAllProducts(int orderId);
        Task<Product?> GetProduct(int productId);
        Task<Product?> GetProduct(int productId, int orderId);
        Task UpdateProduct(int id, Product product);
        Task DeleteProduct(Product product);
        Task CreateProduct(Product newProduct);
        Task RemoveProductFromOrder(int productId);
        Task AddExistingProductToOrder(Product product);
    }

    public class ProductService : IProductService
    {
        private readonly StoreContext context;

        public ProductService(StoreContext context)
        {
            this.context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product?> GetProduct(int productId)
        {
            var product = await context.Products.FindAsync(productId);
            return product;
        }

        public async Task CreateProduct(Product newProduct)
        {
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task UpdateProduct(int id, Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProducts(int orderId)
        {
            var order = await context.Orders.Include(p => p.Products).Where(o => o.Id == orderId).FirstOrDefaultAsync();
            return order.Products;
        }

        public async Task<Product?> GetProduct(int productId, int orderId)
        {
            return await context.Products.Where(o => o.Id == productId && o.OrderId == orderId).FirstOrDefaultAsync();
        }

        public async Task RemoveProductFromOrder(int productId)
        {
            var product = context.Products.Where(o => o.Id == productId).FirstOrDefault();
            product.OrderId = null;
            await context.SaveChangesAsync();
        }

        public async Task AddExistingProductToOrder(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }
    }
}