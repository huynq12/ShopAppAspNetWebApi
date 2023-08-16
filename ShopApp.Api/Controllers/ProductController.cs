using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopApp.Api.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DTOs;
using System.Net.WebSockets;
using System.IO;


namespace ShopApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IImageRepository _imageRepository;

        public ProductController(IProductRepository productRepository,
			ICategoryRepository categoryRepository,
            IWebHostEnvironment hostingEnvironment,
            IImageRepository imageRepository)
        {
            _productRepository = productRepository;
			_categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _imageRepository = imageRepository;
        }
		[HttpGet("/products")]
		public async Task<IActionResult> GetProducts()
		{
			var list = await _productRepository.GetAllProducts();
            var productIds = list.Select(x=>x.Id).ToArray();
            var images = await _imageRepository.GetImageByProductIds(productIds);
			var listDto = list.Select(x => new ProductDto
			{
				Id = x.Id,
				Name = x.Name,
				Quantity = x.Quantity,
                SoldQuantity = x.SoldQuantity,
				Price = x.Price.ToString(),
				Description = x.Description,
                ImageUrl = images.FirstOrDefault(c=>c.ProductId==x.Id) == null ? "" : $"https://localhost:7000/images/product/{x.Id}/{images.FirstOrDefault(c => c.ProductId == x.Id).ImageName}",
			});
			var recordsTotal = list.Count();
			var data = new { data = listDto, recordsTotal };
			return Ok(data);
		}

		[HttpGet("/product/{id}")]
		public async Task<IActionResult> GetOne(int id)
		{
			var result = await _productRepository.GetProductById(id);
			if(result == null)
				return NotFound();
			return Ok(result);
		}
		[HttpGet("/view-product/{id}")]
		public async Task<IActionResult> ViewProduct(int id)
		{
            var listCategoryIds = await _categoryRepository.GetCategoryIdsByProductId(id);

            var product = await _productRepository.GetProductById(id);
            
            if (product == null)
            {
                return NotFound();
            }

            return Ok(new ProductDetailDto
            {
                Id = id,
                Name =	product.Name,
				Quantity = product.Quantity,
				Price= product.Price,
				Description = product.Description,
				RAM = product.RAM,
				HardDrive = product.HardDrive,
				Screen = product.Screen,
				CPU	= product.CPU,
				Power = product.Power,
                CategoryIds = listCategoryIds.ToArray()
            });
        }

        [HttpPost("/create-product")]
		public async Task<IActionResult> Create([FromBody]AddProductRequest request)
		{
            
            var product = new Product()
			{
				Name = request.Name,
				Quantity = request.Quantity,
                SoldQuantity = 0,
				Price = request.Price,
				Description = request.Description,
				RAM = request.RAM,
				HardDrive = request.HardDrive,
				Screen = request.Screen,
				CPU = request.CPU,
				Power = request.Power,
                //Image = request.Image,
				CreatedAt = DateTime.Now,
			};
            if(product.Price <= 0 || product.Quantity <= 0)
            {
                return BadRequest();
            }
            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                foreach (var categoryId in request.CategoryIds)
                {
                    product.ProductCategories.Add(new ProductCategory
                    {
                        CategoryId = categoryId,
                        ProductId = request.Id,
                    });
                }
            }
			var result = await _productRepository.Create(product);
            return Ok(result);
        }
        [HttpPut("/edit-product")]
        public async Task<IActionResult> Update(AddProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _productRepository.GetProductById(request.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = request.Name;
            existingProduct.Quantity = request.Quantity;
            existingProduct.Price = request.Price;
            existingProduct.Description = request.Description;
            existingProduct.RAM = request.RAM;
            existingProduct.CPU = request.CPU;
            existingProduct.Power = request.Power;
            existingProduct.Screen = request.Screen;
            existingProduct.HardDrive = request.HardDrive;
            if(existingProduct.Price <= 0 || existingProduct.Quantity < 0)
            {
                return BadRequest();
            }
            await _categoryRepository.DeleteAllProductCategories(request.Id);

            List<ProductCategory> productCategories = new List<ProductCategory>();
            if (request.CategoryIds != null && request.CategoryIds.Any())
            {

                foreach (var categoryId in request.CategoryIds)
                {
                    productCategories.Add(new ProductCategory
                    {
                        CategoryId = categoryId,
                        ProductId = request.Id
                    });

                }
                existingProduct.ProductCategories = productCategories;
            }

            var updatedProduct = await _productRepository.Update(existingProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("/delete-product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var deletedProduct = await _productRepository.Delete(product);
            return Ok(deletedProduct);
        }

    }
}
