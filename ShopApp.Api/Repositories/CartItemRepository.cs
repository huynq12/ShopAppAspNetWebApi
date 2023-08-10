using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;

namespace ShopApp.Api.Repositories
{
	public class CartItemRepository : ICartItemRepository
	{
		private readonly DataContext _context;

		public CartItemRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<CartItem> Create(CartItem cartItem)
		{
			_context.CartItems.Add(cartItem);
			await _context.SaveChangesAsync();
			return cartItem;
		}

		public async Task<CartItem> Delete(CartItem cartItem)
		{
			_context.CartItems.Remove(cartItem);
			await _context.SaveChangesAsync();
			return cartItem;
		}

		public async Task<List<CartItem>> GetCartItems(int shoppingCartId)
		{
			return await _context.CartItems.Where(x => x.ShoppingCartId == shoppingCartId).ToListAsync();
		}

		public async Task<CartItem> Update(CartItem cartItem)
		{
			_context.CartItems.Update(cartItem);
			await _context.SaveChangesAsync();
			return cartItem;
		}
	}
}
