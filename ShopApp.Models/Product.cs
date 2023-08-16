using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int SoldQuantity { get; set; }
		public decimal Price { get; set; }
        public string Description { get; set; }
        public string CPU { get; set; }
		public string Screen { get; set; }
		public string RAM { get; set; }
		public string HardDrive { get; set; }
		public string Power { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
		public List<Review> Reviews { get; set; } = new List<Review>();
		public List<Image> Images { get; set; } = new List<Image>();
	}
}
