using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;

namespace ShopApp.Api.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext _context;

        public ImageRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<Image> Create(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<List<Image>> Delete(int productId)
        {
            var list = await _context.Images.Where(x => x.ProductId == productId).ToListAsync();
            _context.Images.RemoveRange(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public Task<List<Image>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Image>> GetImageByProductIds(int[] productIds)
        {
            var images = await _context.Images.Where(x=> productIds.Contains(x.ProductId)).ToListAsync();
            return images;
        }

        public async Task<List<string>> GetImageNames(int productId)
        {
            List<string> imageNames = new List<string>();
            var listImages = await _context.Images.Where(x => x.ProductId == productId).ToListAsync();
            foreach(var item in listImages)
            {
                imageNames.Add(item.ImageName);
            }
            return imageNames;

        }

        public async Task<List<Image>> GetImagesByProductId(int productId)
        {
            return await _context.Images.Where(x => x.ProductId == productId).ToListAsync();
        }
    }
}
