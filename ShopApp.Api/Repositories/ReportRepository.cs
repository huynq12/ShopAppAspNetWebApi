using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _context;

        public ReportRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<ReportModel<Category, int>> GetTopCategories(int num)
        {
            var topCategories = _context.ProductCategories
                         .GroupBy(pc => pc.CategoryId) 
                         .Select(g => new { CategoryId = g.Key, Count = g.Count() }) 
                         .OrderByDescending(x => x.Count)
                         .Take(num)
                         .Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => new { Category = c, pc.Count }); // Kết hợp với bảng danh mục để lấy danh mục thay vì CategoryId

            List<Category> list = new List<Category>();
            List<int> count = new List<int>();
            foreach (var category in topCategories)
            {
                list.Add(category.Category);
                count.Add(category.Count);
            }
            return new ReportModel<Category, int>(list, count);
        }

        public async Task<ReportModel<Product, int>> GetTopProducts(int num)
        {
            var topSoldProducts = _context.Products
                .OrderByDescending(product => product.SoldQuantity)
                .Take(num)
                .ToList();
            List<int> soldList = new List<int>();
            foreach(var product in topSoldProducts)
            {
                soldList.Add(product.SoldQuantity);
            }
            return new ReportModel<Product, int>(topSoldProducts, soldList);
        }

        public async Task<ReportModel<Product, double>> GetTopReviewProducts(int num)
        {
            var topRatedProducts = _context.Products
            .OrderByDescending(product => product.Reviews.Average(review => review.Rating))
            .Take(num)
            .ToList();
            List<double> ratingValueList = new List<double>();
            foreach(var product in topRatedProducts)
            {
                double averageRating = product.Reviews.Average(review => review.Rating);
                ratingValueList.Add(averageRating);
            }
            return new ReportModel<Product, double>(topRatedProducts, ratingValueList);

        }
    }
}
