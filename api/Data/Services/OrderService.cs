using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Services
{
    public interface IOrderService
    {
        Task CreateOrder(Order newOrder);
        Task DeleteOrder(Order Order);
        Task<List<Order>> GetAllOrders();
        Task<Order?> GetOrder(int OrderId);
        Task UpdateOrder(int id, Order Order);
    }

    public class OrderService : IOrderService
    {
        private StoreContext context;

        public OrderService(StoreContext context)
        {
            this.context = context;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            //return await context.Orders.Include(p => p.Products).ToListAsync();
            return await context.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrder(int orderId)
        {
            var order = await context.Orders.FirstOrDefaultAsync(i => i.Id == orderId);
            //var order = await context.Orders.Include(p => p.Products).FirstOrDefaultAsync(i => i.Id == orderId);
            return order;
        }

        public async Task CreateOrder(Order newOrder)
        {
            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order Order)
        {
            context.Orders.Remove(Order);
            await context.SaveChangesAsync();
        }

        public async Task UpdateOrder(int id, Order Order)
        {
            context.Orders.Update(Order);
            await context.SaveChangesAsync();
        }
    }
}
