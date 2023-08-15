using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
    public class UploadImageRequest
    {
        public int ProductId { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
