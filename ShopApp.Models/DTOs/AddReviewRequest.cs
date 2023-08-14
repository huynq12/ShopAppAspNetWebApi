using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
	public class AddReviewRequest
	{
		public string UserName { get; set; }
		public int OrderDetailId { get; set; }
		public string CommentMsg { get; set; }
		public int Rating { get; set; }
	}
}
