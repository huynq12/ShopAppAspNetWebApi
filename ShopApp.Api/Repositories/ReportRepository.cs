using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        public async Task<ReportModel<Category, int>> GetTopCategories()
        {
            var topCategories = _context.ProductCategories
                         .GroupBy(pc => pc.CategoryId) 
                         .Select(g => new { CategoryId = g.Key, Count = g.Count() }) 
                         .OrderByDescending(x => x.Count)
                         .Take(5)
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

        public async Task<ReportModel<Product, int>> GetTopProducts()
        {
            var topSoldProducts = _context.Products
                .OrderByDescending(product => product.SoldQuantity)
                .Take(5)
                .ToList();
            List<int> soldList = new List<int>();
            foreach(var product in topSoldProducts)
            {
                soldList.Add(product.SoldQuantity);
            }
            return new ReportModel<Product, int>(topSoldProducts, soldList);
        }

        public async Task<ReportModel<Product, double>> GetTopReviewProducts()
        {
            var topProducts = _context.Reviews
                .GroupBy(c => c.OrderDetail.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    AverageRating = g.Average(c => c.Rating)
                })
                .OrderByDescending(g => g.AverageRating)
                .Take(5)
                .Join(_context.Products, cm => cm.ProductId, p => p.Id, (cm, p) => new { Product = p, AverageRate = cm.AverageRating });


            List<Product> list = new List<Product>();
            List<double> averageRatingList = new List<double>();

            foreach (var p in topProducts)
            {
                list.Add(p.Product);
                averageRatingList.Add(p.AverageRate);
            }

            return new ReportModel<Product, double>(list, averageRatingList);
        }
    }
}
