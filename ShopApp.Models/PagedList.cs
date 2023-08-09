using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
	public class PagedList<T>
	{
		public List<T> Items { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public PagedList(List<T> items, int count, int pageSize)
		{

			TotalCount = count;
			PageSize = pageSize;
			Items = items;
		}
	}
}
