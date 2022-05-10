using Microsoft.AspNetCore.Mvc.Rendering;

using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.ViewModels
{
	public class ProductVM
	{
		public Product Product { get; set; }
		public IEnumerable<SelectListItem> CategorySelectedList { get; set; }
	}
}
