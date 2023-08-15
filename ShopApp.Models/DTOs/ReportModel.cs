using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
    public class ReportModel<T,U>
    {
        public List<T> Items { get; set; }
        public List<U> ListCount { get; set; }
        public ReportModel(List<T> items, List<U> listCount)
        {
            Items = items;
            ListCount = listCount;
        }

    }
}
