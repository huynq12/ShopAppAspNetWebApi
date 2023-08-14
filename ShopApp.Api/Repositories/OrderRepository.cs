using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;

namespace ShopApp.Api.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DataContext _context;

		public OrderRepository(DataContext dataContext) {
			_context = dataContext;
		}

		public async Task<Order> Create(Order order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
			return order;
		}

		public async Task<Order> Delete(Order order)
		{
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
			return order;
		}

		public async Task<Order> GetOrderById(int id)
		{
			return await _context.Orders.Include(x=>x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);
		}

		public Task<OrderDetail> GetOrderDetailById(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<Order>> GetOrders()
		{
			return await _context.Orders.Include(x=>x.OrderDetails).ToListAsync();
		}

		public async Task<List<Order>> GetOrdersByUser(string user)
		{
			return await _context.Orders.Where(x=>x.User==user).ToListAsync();
		}

		public async Task<Order> Update(Order order)
		{
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
			return order;
		}
	}
}
