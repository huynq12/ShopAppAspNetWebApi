using ShopApp.Models;

namespace ShopApp.Api.Interfaces
{
	public interface IShoppingCartRepository
	{
		Task<ShoppingCart> GetShoppingCartByUser(string user);
		Task<ShoppingCart> Create(ShoppingCart shoppingCart);
		Task<ShoppingCart> Delete(ShoppingCart shoppingCart);
		Task<bool> HasCart(string user);
	}
}
