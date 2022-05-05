using Microsoft.EntityFrameworkCore;

using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.DataContext
{
	public class MssqlContext:DbContext
	{
		public MssqlContext(DbContextOptions<MssqlContext> options) : base(options)
		{

		}
	public DbSet<Address> Addresses { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		
	}
}
