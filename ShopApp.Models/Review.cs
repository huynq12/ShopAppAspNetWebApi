using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
	public class Review
	{
		public int Id { get; set; }	
		public string User { get; set; }
		public string UserName { get; set; }
		public int OrderDetailId { get; set; }
		public OrderDetail OrderDetail { get; set; }
		public string CommentMsg { get; set; }
		public int Rating { get; set; }
	}
}
