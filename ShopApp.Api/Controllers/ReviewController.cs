using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Api.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewRepository _reviewRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IOrderRepository _orderRepository;

		public ReviewController(IReviewRepository reviewRepository,
			IHttpContextAccessor httpContextAccessor,
			IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
			_httpContextAccessor = httpContextAccessor;
			_orderRepository = orderRepository;
        }

		[HttpGet("/product/reviews/{orderDetailId}")]
		public async Task<IActionResult> GetReviews(int orderDetailId) {
			var list = await _reviewRepository.GetReviews(orderDetailId);
			return Ok(list);
		}

		[HttpPost("/place-review")]
		public async Task<IActionResult> PlaceReview([FromBody] AddReviewRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (user == null)
				return BadRequest();

			var orderDetail = await _orderRepository.GetOrderDetailById(request.OrderDetailId);
			if (orderDetail == null)
				return BadRequest();

			var order = await _orderRepository.GetOrderById(orderDetail.OrderId);
			if (order == null || order.Status == OrderStatus.Cancel || order.Status == OrderStatus.Processing || order.User != user)
				return BadRequest();

			if (await _reviewRepository.HasReviewed(user, request.OrderDetailId))
			{
				return BadRequest();
			}

			var review = new Review()
			{
				User = user,
				UserName = request.UserName,
				OrderDetailId = request.OrderDetailId,
				CommentMsg = request.CommentMsg,
				Rating = request.Rating
			};

			await _reviewRepository.Create(review);
			return Ok(review);
		}
	}
}
