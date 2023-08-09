using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
	public class Filter
	{
		public int Start { get; set; }
		public int Length { get; set; }
		public string? SearchValue { get; set; }
		public string? Sort { get; set; }
	}
}
