using ShopApp.Models;

namespace ShopApp.Api.Interfaces
{
	public interface ICartItemRepository
	{
		Task<List<CartItem>> GetCartItems(int shoppingCartId);
		Task<CartItem> Create(CartItem cartItem);
		Task<CartItem> Update(CartItem cartItem);
		Task<CartItem> Delete(CartItem cartItem);
	}
}
