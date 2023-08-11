﻿using ShopApp.Models;

namespace ShopApp.Api.Interfaces
{
	public interface IOrderRepository
	{
		Task<List<Order>> GetOrders();
		Task<Order> GetOrderById(int id);
		Task<Order> Create(Order order);
		Task<Order> Update(Order order);
		Task<Order> Delete(Order order);
	}
}
