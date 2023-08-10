using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;

namespace ShopApp.Api.Repositories
{
	public class ShoppingCartRepository : IShoppingCartRepository
	{
		private readonly DataContext _context;

		public ShoppingCartRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<ShoppingCart> Create(ShoppingCart shoppingCart)
		{
			_context.ShoppingCarts.Add(shoppingCart);
			await _context.SaveChangesAsync();
			return shoppingCart;
		}

		public async Task<ShoppingCart> Delete(ShoppingCart shoppingCart)
		{
			_context.ShoppingCarts.Remove(shoppingCart);
			await _context.SaveChangesAsync();
			return shoppingCart;
		}

		public async Task<ShoppingCart> GetShoppingCartByUser(string user)
		{
			return await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.User == user);
		}

		public async Task<bool> HasCart(string user)
		{
			var check = _context.ShoppingCarts.Any(x => x.User == user);
			return check;
		}
	}
}
