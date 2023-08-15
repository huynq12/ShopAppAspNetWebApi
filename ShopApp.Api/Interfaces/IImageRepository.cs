using ShopApp.Models;

namespace ShopApp.Api.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> GetImageByProductId(int productId);
        Task<Image> Create(Image image);
        Task<Image> Delete(Image image);
    }
}
