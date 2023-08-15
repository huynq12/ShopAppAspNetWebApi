using ShopApp.Api.Interfaces;
using ShopApp.Models;

namespace ShopApp.Api.Repositories
{
    public class ImageRepository : IImageRepository
    {
        public Task<Image> Create(Image image)
        {
            throw new NotImplementedException();
        }

        public Task<Image> Delete(Image image)
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetImageByProductId(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
