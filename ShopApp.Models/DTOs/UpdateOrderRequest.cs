
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
	public class UpdateOrderRequest
	{
		public int OrderId { get; set; }
		public string? UserName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Address { get; set; }
		public OrderStatus? Status { get; set; }

	}
}
