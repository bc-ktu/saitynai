using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Services
{
    public interface IOrderService
    {
        Task CreateOrder(Order newOrder);
        Task DeleteOrder(Order Order);
        Task<List<Order>> GetAllOrders();
        Task<List<Order>> GetAllUsersOrders(string id);
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
            return await context.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrder(int orderId)
        {
            var order = await context.Orders.FirstOrDefaultAsync(i => i.Id == orderId);
            return order;
        }

        public async Task CreateOrder(Order newOrder)
        {
            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order order)
        {
            var getOrder = await context.Orders
                .Include(p => p.Products)
                .FirstOrDefaultAsync(i => i.Id == order.Id);
            if (getOrder.Products.Count != 0)
            {
                foreach (var p in getOrder.Products)
                    p.OrderId = null;
            }
            
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }

        public async Task UpdateOrder(int id, Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllUsersOrders(string id)
        {
            return await context.Orders.Where(a => a.OrdererId == id).ToListAsync();
        }
    }
}
