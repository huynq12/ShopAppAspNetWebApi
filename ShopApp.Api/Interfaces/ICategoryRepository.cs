using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Interfaces
{
	public interface ICategoryRepository
	{
		//Task<PagedList<Category>> GetCategories(Filter filter);	
		Task<List<Category>> GetAllCategories();
		Task<List<Category>> GetCategoriesByProductId(int postId);
		Task<List<int>> GetCategoryIdsByProductId(int productId);
		Task<Category> GetCategoryById(int id);
		Task<Category> Create(Category category);
		Task<Category> Update(Category category);
		Task<Category> Delete(Category category);
		Task<List<ProductCategory>> DeleteAllProductCategories(int productId);
	}
}
