using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopApp.Models;

namespace ShopApp.Api.Data
{
	public class DataContext : IdentityDbContext<ApplicationUser>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetail { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Image> Images { get; set; }
		
		protected override void OnModelCreating(ModelBuilder model)
		{
			model.Entity<ProductCategory>().HasKey(pt => new { pt.ProductId, pt.CategoryId });

			model.Entity<ProductCategory>()
				.HasOne(p => p.Product)
				.WithMany(pt => pt.ProductCategories)
				.HasForeignKey(p => p.ProductId);
			model.Entity<ProductCategory>()
				.HasOne(t => t.Category)
				.WithMany(pt => pt.ProductCategories)
				.HasForeignKey(t => t.CategoryId);

			base.OnModelCreating(model);
		}
	}
}
