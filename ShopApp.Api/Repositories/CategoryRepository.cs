using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
	{
		private readonly DataContext _context;

		public CategoryRepository(DataContext dataContext)
        {
            _context  = dataContext;
        }
        public async Task<Category> Create(Category category)
		{
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();
			return category;
		}

		public async Task<Category> Delete(Category category)
		{
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
			return category;
		}

        public async Task<List<ProductCategory>> DeleteAllProductCategories(int productId)
        {
            var listProductCategories = await _context.ProductCategories.Where(x => x.ProductId == productId).ToListAsync();
            _context.ProductCategories.RemoveRange(listProductCategories);
            await _context.SaveChangesAsync();
            return listProductCategories;
        }

        public async Task<List<Category>> GetAllCategories()
		{
			return await _context.Categories.ToListAsync();
		}

		/*public async Task<List<Category>> GetCategories(F)
		{
			var list = _context.Categories.ToListAsync();
			//var recordsTotal = list.Count();
			return list
		}*/

        public async Task<List<Category>> GetCategoriesByProductId(int productId)
        {
            var category = from pt in _context.ProductCategories
                      join c in _context.Categories on pt.CategoryId equals c.Id
                      where pt.ProductId == productId
                      select c;
            return await category.ToListAsync();
        }

        public async Task<List<int>> GetCategoryIdsByProductId(int productId)
        {
            List<int> categoryIds = new List<int>();
            var listProductCateogries = await _context.ProductCategories.Where(x => x.ProductId == productId).ToListAsync();
            foreach (var item in listProductCateogries)
            {
                categoryIds.Add(item.CategoryId);
            }
            return categoryIds;
        }

        public async Task<Category> GetCategoryById(int id)
		{
			return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Category> Update(Category category)
		{
			_context.Categories.Update(category);
			await _context.SaveChangesAsync();
			return category;
		}
	}
}
