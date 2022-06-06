using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using UpakModelsLibrary.Models;

namespace UpakDataAccessLibrary.DataContext
{
	public class MssqlContext:IdentityDbContext
	{
		public MssqlContext(DbContextOptions<MssqlContext> options) : base(options)
		{

		}
		public DbSet<Address> Addresses { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductOrder> ProductOrders { get; set; }
		public DbSet<UltrapackUser> UltrapackUsers { get; set; }
	}
}
