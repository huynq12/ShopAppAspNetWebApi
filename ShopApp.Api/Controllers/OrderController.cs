using Microsoft.AspNetCore.Authorization;
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

		[Authorize(Roles ="Admin")]
		[HttpGet("/orders")]
		public async Task<IActionResult> GetOrders()
		{
			var list = await _orderRepository.GetOrders();
			return Ok(list);
		}

		[Authorize]
		[HttpGet("/order/{id}")]
		public async Task<IActionResult> GetOrder(int id)
		{
			var order = await _orderRepository.GetOrderById(id);
			if (order == null)
				return NotFound();
			return Ok(order);
		}

		[Authorize]
		[HttpGet("/user-orders")]
		public async Task<IActionResult> GetOrdersByUser()
		{
			var user = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (user == null)
				return BadRequest();
			var listOrders = await _orderRepository.GetOrdersByUser(user);
			return Ok(listOrders);
		}

		[Authorize(Roles ="Admin")]
		[HttpGet("/orders-by/{userEmail}")]
		public async Task<IActionResult> GetOrdersByUser(string userEmail)
		{
			var listOrders = await _orderRepository.GetOrdersByUser(userEmail);
			return Ok(listOrders);
		}

		[Authorize]
		[HttpPost("/place-order")]
		public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (user == null)
				return BadRequest("Unauthorized");

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
						return NotFound("The product is not exist");

					if(product.Quantity < cartItem.Quantity)
						return BadRequest("The product quantity is not enough");

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


		[Authorize(Roles ="Admin")]
		[HttpPut("/update-order-admin")]
		public async Task<IActionResult> Update(UpdateOrderRequest request)
		{
			var existingOrder = await _orderRepository.GetOrderById(request.OrderId);
			if (existingOrder == null)
				return NotFound("Cannot find the order");

			if (existingOrder.Status == OrderStatus.Complete || existingOrder.Status == OrderStatus.Cancel || existingOrder.Status == OrderStatus.Shipped)
				return BadRequest("Invalid order");

			switch (request.Status)
			{

				case OrderStatus.Processing:
                    foreach (var item in existingOrder.OrderDetails)
                    {
                        var product = await _productRepository.GetProductById(item.ProductId);
                        if (product == null)
                            return NotFound("Cannot find the product");

                        if (product.Quantity < item.Amount)
                            return BadRequest("The product quantity is not valid");
                    }
                    existingOrder.Status = OrderStatus.Processing;
					break;
				case OrderStatus.Shipping:
                    foreach (var item in existingOrder.OrderDetails)
                    {
                        var product = await _productRepository.GetProductById(item.ProductId);
                        if (product == null)
                            return NotFound("Cannot find the product");

                        if (product.Quantity < item.Amount)
                            return BadRequest("The product quantity is not valid");
                    }
                    existingOrder.Status = OrderStatus.Shipping;
                    break;
                case OrderStatus.Shipped:
                    foreach (var item in existingOrder.OrderDetails)
                    {
                        var product = await _productRepository.GetProductById(item.ProductId);
                        if (product == null)
                            return NotFound();

                        if (product.Quantity < item.Amount)
                            return BadRequest("The product quantity is not valid");

                        product.Quantity -= item.Amount;
                        product.SoldQuantity += item.Amount;
                        await _productRepository.Update(product);
                    }
                    existingOrder.Status = OrderStatus.Shipped;
					break;
			}
			await _orderRepository.Update(existingOrder);
			return Ok("Update order successfully");
		}


		[Authorize]
		[HttpPut("/update-order-user")]
		public async Task<IActionResult> UpdateUserOrder(UpdateOrderRequest request)
		{
            var existingOrder = await _orderRepository.GetOrderById(request.OrderId);
            if (existingOrder == null)
                return NotFound();

            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (user == null || user != existingOrder.User)
                return BadRequest("Unauthorized");

            if (existingOrder.Status == OrderStatus.Complete || existingOrder.Status == OrderStatus.Cancel)
                return BadRequest();

			switch (request.Status)
			{
				case OrderStatus.Complete:
					if (existingOrder.Status != OrderStatus.Shipped)
						return BadRequest();
					existingOrder.Status = OrderStatus.Complete;
					break;

				case OrderStatus.Cancel:
					if (existingOrder.Status == OrderStatus.Shipped)
						return BadRequest();
					existingOrder.Status = OrderStatus.Cancel;
					break;
			}
            await _orderRepository.Update(existingOrder);
            return Ok("Update order successfully");
        }


		[Authorize(Roles ="Admin")]
		[HttpDelete("/delete-order/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var order = await _orderRepository.GetOrderById(id);
			if (order == null)
				return NotFound();

			var deletedOrder = await _orderRepository.Delete(order);
			
			return Ok(deletedOrder);
		}

    }
}
