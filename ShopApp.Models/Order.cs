using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string User { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		
	}
}
