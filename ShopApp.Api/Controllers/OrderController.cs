using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Api.Interfaces;
using ShopApp.Api.Migrations;
using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;
		
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IProductRepository _productRepository;

		public OrderController(IOrderRepository orderRepository,
			
			IHttpContextAccessor httpContextAccessor,
			IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
		
			_httpContextAccessor = httpContextAccessor;
			_productRepository = productRepository;
        }

		[HttpGet("/orders")]
		public async Task<IActionResult> GetOrders()
		{
			var list = await _orderRepository.GetOrders();
			return Ok(list);
		}

		[HttpPost("/place-order")]
		public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (user == null)
				return BadRequest();

			var order = new Order()
			{
				User = user,
				UserName = request.FullName,
				PhoneNumber = request.PhoneNumber,
				Address = request.Address,
				OrderDate = DateTime.Now,
				Status = OrderStatus.Processing
			};
			if(request.CartItems.Any())
			{
				foreach(var cartItem in request.CartItems)
				{
					var product = await _productRepository.GetProductById(cartItem.ProductId);
					if (product == null)
						return NotFound();

					if(product.Quantity < cartItem.Quantity)
						return BadRequest();

					order.OrderDetails.Add(new OrderDetail
					{
						OrderId = order.Id,
						ProductId = cartItem.ProductId,
						Amount = cartItem.Quantity,
						Price = product.Price
					});
					order.TotalAmount += product.Price * cartItem.Quantity;
				}
			}
			var result = await _orderRepository.Create(order);
			return Ok(result);
		}

		[HttpPut("/update-order")]
		public async Task<IActionResult> Update(UpdateOrderRequest request)
		{
			var existingOrder = await _orderRepository.GetOrderById(request.OrderId);
			if (existingOrder == null)
				return NotFound();

			if (existingOrder.Status == OrderStatus.Success || existingOrder.Status == OrderStatus.Cancel)
				return BadRequest();

			switch (request.Status)
			{
				case OrderStatus.Success:
					existingOrder.Status = OrderStatus.Success;
					foreach (var item in existingOrder.OrderDetails)
					{
						var product = await _productRepository.GetProductById(item.ProductId);
						if (product == null)
							return NotFound();

						if (product.Quantity < item.Amount)
							return BadRequest();
						product.Quantity -= item.Amount;
						await _productRepository.Update(product);
					}
					break;
				case OrderStatus.Cancel:
					existingOrder.Status = OrderStatus.Cancel;
					break;
				default:
					break;
			}
			existingOrder.UserName = request.UserName;
			existingOrder.Address = request.Address;
			existingOrder.PhoneNumber = request.PhoneNumber;

			var updatedOrder = await _orderRepository.Update(existingOrder);
			return Ok(updatedOrder);
		}

		[HttpDelete("/delete-order/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var order = await _orderRepository.GetOrderById(id);
			if (order == null)
				return NotFound();
			/*foreach (var item in order.OrderDetails)
			{
				var product = await _productRepository.GetProductById(item.ProductId);
				if (product == null)
					return NotFound();
				product.Quantity += item.Amount;
				await _productRepository.Update(product);
			}*/
			var deletedOrder = await _orderRepository.Delete(order);
			
			return Ok(deletedOrder);
		}

    }
}
