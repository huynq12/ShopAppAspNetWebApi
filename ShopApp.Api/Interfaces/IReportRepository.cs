using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Interfaces
{
    public interface IReportRepository
    {
        Task<ReportModel<Category, int>> GetTopCategories(int num);
        Task<ReportModel<Product,int>> GetTopProducts(int num);
        Task<ReportModel<Product, double>> GetTopReviewProducts(int num);

    }
}
