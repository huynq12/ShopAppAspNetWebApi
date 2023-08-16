using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
    public class ImageDto
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDescription { get; set; }
    }
}
