using ShopApp.Models;

namespace ShopApp.Api.Interfaces
{
	public interface IReviewRepository
	{
		Task<List<Review>> GetReviews(int productId);
		Task<Review> GetReview(int orderDetailId);
		Task<Review> Create(Review review);
		Task<Review> Delete(Review review);
		Task<bool> HasReviewed(string user, int orderDetailId);
	}
}
