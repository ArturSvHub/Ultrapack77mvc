using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.DataContext
{
	public class MssqlContext:IdentityDbContext
	{
		public MssqlContext(DbContextOptions<MssqlContext> options) : base(options)
		{

		}
		public DbSet<Address> Addresses { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<UltrapackUser> UltrapackUsers { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
	}
}
