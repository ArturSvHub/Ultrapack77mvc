using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.ViewModels
{
	public class ProductUserVM
	{
		public ProductUserVM()
		{
			ProductList = new List<Product>();
		}
		public IList<Product> ProductList { get; set; }
        public UltrapackUser ApplicationUser { get; set; }
    }
}
