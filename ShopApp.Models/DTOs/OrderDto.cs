using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
	public class OrderDto
	{
		public int OrderId { get; set; }
		public string User { get; set; }
		public string Address { get; set; }
		public CartItem[] CartItems { get; set; }
	}
}
