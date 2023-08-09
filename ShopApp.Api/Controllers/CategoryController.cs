﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ShopApp.Api.Interfaces;
using ShopApp.Models.DTOs;

namespace ShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("/categories")]
        public async Task<IActionResult> Get()
        {
            var list = await _categoryRepository.GetAllCategories();
            return Ok(list);
        }
        [HttpGet("/category/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var result = await _categoryRepository.GetCategoryById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("/categories-by/{productId}")]
        public async Task<IActionResult> GetCategoriesByProductId(int productId)
        {
            var list = await _categoryRepository.GetCategoriesByProductId(productId);
            var result = list.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name
            });
            return Ok(result);
        }

        [HttpPost("/create-category")]
        public async Task<IActionResult> Create([FromBody]CategoryDto request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _categoryRepository.Create(new Models.Category
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            });
            return Ok(result);
        } 

    }
}
