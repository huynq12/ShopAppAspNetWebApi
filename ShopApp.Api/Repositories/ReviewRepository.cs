using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;

namespace ShopApp.Api.Repositories
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly DataContext _context;

		public ReviewRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<Review> Create(Review review)
		{
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();
			return review;

		}

		public async Task<Review> Delete(Review review)
		{
			_context.Reviews.Remove(review);
			await _context.SaveChangesAsync();
			return review;
		}

		public async Task<Review> GetReview(int orderDetailId)
		{
			return await _context.Reviews.FirstOrDefaultAsync(x => x.OrderDetailId == orderDetailId);
		}

		public async Task<List<Review>> GetReviews(int productId)
		{
			return await _context.Reviews.Where(x=>x.OrderDetail.ProductId == productId).ToListAsync();
		}

		public async Task<bool> HasReviewed(string user, int orderDetailId)
		{
			var check = _context.Reviews.Any(x => x.User == user && x.OrderDetailId == orderDetailId);
			return check;
		}
	}
}
