using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApp.Api.Interfaces;
using ShopApp.Api.Repositories;
using ShopApp.Models;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageController(IImageRepository imageRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _imageRepository = imageRepository;
            _hostingEnvironment = webHostEnvironment;
        }
        [NonAction]
        private string GetFilePath(int productId)
        {
            return _hostingEnvironment.WebRootPath + "\\images\\product\\" + productId;
        }
        [HttpGet("/images/{productId}")]
        public async Task<IActionResult> GetImages(int productId)
        {
            var list = await _imageRepository.GetImagesByProductId(productId);
            return Ok(list);
        }
        [HttpGet("/image/{productId}")]
        public async Task<IActionResult> GetImage(int productId)
        {

            List<string> Imageurl = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productId);

                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + "/images/product/" + productId + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }
        [HttpPost("/upload-image")]
        public async Task<IActionResult> UploadImage([FromForm]IFormCollection data)
        {
            var productId = 0;
            var isProductId = int.TryParse(data["productId"], out productId);
            if (!isProductId)
                return BadRequest();
            var files = data.Files;
            int passCount = 0;
            string response = string.Empty;
            try
            {
                string Filepath = GetFilePath(productId);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                foreach (var file in files)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passCount++;
                        var newImage = new Image()
                        {
                            ProductId = productId,
                            ImageName = file.FileName
                        };
                        await _imageRepository.Create(newImage);

                    }
                }
                response = passCount.ToString();

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete("/remove-image/{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {

            try
            {
                string Filepath = GetFilePath(productId);
                
                if (!System.IO.Directory.Exists(Filepath))
                {
                    return NotFound();
                }
                DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    fileInfo.Delete();
                }
                await _imageRepository.Delete(productId);
                return Ok("pass");
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

    }
}
