using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CPU { get; set; }
		public string Screen { get; set; }
		public string RAM { get; set; }
		public string HardDrive { get; set; }
		public string Power { get; set; }
		public string Description { get; set; }
		public List<ProductCategory> ProductCategories { get; set; }
	}
}
