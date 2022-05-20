using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc.Models;
using Ultrapack77mvc.Utility.Extentions;
using Ultrapack77mvc.ViewModels;

namespace Ultrapack77mvc.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly MssqlContext _context;
		[BindProperty]
		public ProductUserVM ProductUserVM { get; set; }

		public CartController(MssqlContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
			if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null&&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count()>0)
			{
				shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			List<int> prodInCart = shoppingCartList.Select(i=>i.ProductId).ToList();
			List<Product> prodList = _context.Products.Where(u => prodInCart.Contains(u.Id)).ToList();

			return View(prodList);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Index")]
		public IActionResult IndexPost()
		{
			return RedirectToAction(nameof(Summary));
		}

        public IActionResult Summary()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			//var userId = User.FindFirstValue(ClaimTypes.Name);

			List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
			{
				shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
			List<Product> prodList = _context.Products.Where(u => prodInCart.Contains(u.Id)).ToList();

			ProductUserVM = new()
			{
				ApplicationUser = _context.UltrapackUsers.FirstOrDefault(u => u.Id == claim.Value),
				ProductList = prodList
			};
			return View(ProductUserVM); 
		}

		public IActionResult Remove(int id)
		{
			List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
			{
				shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
			HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

			return RedirectToAction(nameof(Index));
		}
	}
}
