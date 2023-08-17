using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(200, ErrorMessage = "Comment must not exceed 200 characters.")]
        public string CommentMsg { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
	}
}
