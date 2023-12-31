﻿using Microsoft.EntityFrameworkCore;
using ShopApp.Api.Data;
using ShopApp.Api.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DTOs;
using System.Net.WebSockets;

namespace ShopApp.Api.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly DataContext _context;

		public ProductRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<Product> Create(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
			return product;
		}

		public async Task<Product> Delete(Product product)
		{
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return product;
		}

		public async Task<List<Product>> GetAllProducts()
		{
			return await _context.Products.Include(x=>x.Images).ToListAsync();
		}

		public async Task<Product> GetProductById(int id)
		{
            return await _context.Products.Include(x => x.ProductCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<List<Product>> GetProducts(Filter filter)
		{
			var list =  _context.Products.Include(x=>x.ProductCategories).ThenInclude(x=>x.Category).Include(x=>x.Images).AsQueryable();
			if (filter.CategoryId.HasValue)
			{
				list = list.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == filter.CategoryId));
            }
			
			return list.ToList();
		}

		public async Task<Product> Update(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
			return product;
		}
	}
}
