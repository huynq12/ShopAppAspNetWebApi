using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Interfaces
{
	public interface IProductRepository
	{
		Task<List<Product>> GetProducts(Filter filter);
		Task<List<Product>> GetAllProducts();
		Task<Product> GetProductById(int id);
		Task<Product> Create(Product product);
		Task<Product> Update(Product product);
		Task<Product> Delete(Product product);
	}
}
