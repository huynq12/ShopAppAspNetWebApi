using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
    public class ProductWithAvgRating
    {
        public Product Product { get; set; }
        public double AvgRating { get; set; }
    }
}
