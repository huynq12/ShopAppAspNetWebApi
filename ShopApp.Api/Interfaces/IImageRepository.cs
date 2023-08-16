using ShopApp.Models;

namespace ShopApp.Api.Interfaces
{
    public interface IImageRepository
    {
        Task<List<Image>> GetImageByProductIds(int[] productIds);
        Task<List<Image>> GetAllAsync();    
        Task<List<Image>> GetImagesByProductId(int productId);
        Task<List<string>> GetImageNames(int productId);
        Task<Image> Create(Image image);
        Task<List<Image>> Delete(int productId);
    }
}
