
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
		public OrderStatus Status { get; set; }

	}
}
