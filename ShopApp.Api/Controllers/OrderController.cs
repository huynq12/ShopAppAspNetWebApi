using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ShopApp.Api.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IProductRepository _productRepository;

		public OrderController(IOrderRepository orderRepository,
			UserManager<ApplicationUser> userManager,
			IHttpContextAccessor httpContextAccessor,
			IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_productRepository = productRepository;
        }

		[HttpGet("/orders")]
		public async Task<IActionResult> GetOrders()
		{
			var list = await _orderRepository.GetOrders();
			return Ok(list);
		}
		[HttpGet("/order/{id}")]
		public async Task<IActionResult> GetOrder(int id)
		{
			var order = await _orderRepository.GetOrderById(id);
			if (order == null)
				return NotFound();
			return Ok(order);
		}
		[HttpGet("/user-orders")]
		public async Task<IActionResult> GetOrdersByUser()
		{
			var user = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (user == null)
				return BadRequest();
			var listOrders = await _orderRepository.GetOrdersByUser(user);
			return Ok(listOrders);
		}

		[HttpGet("/orders-by")]
		public async Task<IActionResult> GetOrdersByUser(string userEmail)
		{
			var listOrders = await _orderRepository.GetOrdersByUser(userEmail);
			return Ok(listOrders);
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
				Status = OrderStatus.New
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
						ProductName = product.Name,
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

			var user = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (user == null || user != existingOrder.User)
				return BadRequest();

			if (existingOrder.Status == OrderStatus.Complete || existingOrder.Status == OrderStatus.Cancel)
				return BadRequest();

			switch (request.Status)
			{
				case OrderStatus.Processing:
                    foreach (var item in existingOrder.OrderDetails)
                    {
                        var product = await _productRepository.GetProductById(item.ProductId);
                        if (product == null)
                            return NotFound();

                        if (product.Quantity < item.Amount)
                            return BadRequest();
                    }
                    existingOrder.Status = OrderStatus.Processing;
					break;
                case OrderStatus.Shipped:
                    foreach (var item in existingOrder.OrderDetails)
                    {
                        var product = await _productRepository.GetProductById(item.ProductId);
                        if (product == null)
                            return NotFound();

                        if (product.Quantity < item.Amount)
                            return BadRequest();

                        product.Quantity -= item.Amount;
                        product.SoldQuantity += item.Amount;
                        await _productRepository.Update(product);
                    }
                    existingOrder.Status = OrderStatus.Shipped;
					break;
				case OrderStatus.Complete:
					existingOrder.Status = OrderStatus.Complete;
					break;
				case OrderStatus.Cancel:
					existingOrder.Status = OrderStatus.Cancel;
					break;
				
			}
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
