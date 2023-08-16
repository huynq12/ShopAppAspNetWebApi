using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Interfaces
{
    public interface IReportRepository
    {
        Task<ReportModel<Category, int>> GetTopCategories();
        Task<ReportModel<Product,int>> GetTopProducts();
        Task<ReportModel<Product, double>> GetTopReviewProducts();

    }
}
