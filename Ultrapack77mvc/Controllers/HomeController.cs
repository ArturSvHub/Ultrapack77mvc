using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using UpakUtilitiesLibrary;
using UpakDataAccessLibrary.DataContext;
using UpakModelsLibrary.Models;
using UpakUtilitiesLibrary.Utility.Extentions;
using UpakModelsLibrary.Models.ViewModels;

namespace Ultrapack77mvc.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly MssqlContext _context;
		public HomeController(ILogger<HomeController> logger, MssqlContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			HomeVM homeVM = new HomeVM()
			{
				Products = _context.Products.Include(u => u.Category),
				Categories = _context.Categories
			};
			return View(homeVM);
		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult About()
		{
			return View();
		}
		public IActionResult Contacts()
		{
			return View();
		}
		public IActionResult Providers()
		{
			return View();
		}
		public IActionResult Services()
		{
			return View();
		}
		public IActionResult Wiki()
		{
			return View();
		}
		public IActionResult Details(int id)
		{

			HomeVM homeVM = new HomeVM()
			{
				Products = _context.Products.Include(u => u.Category),
				Categories = _context.Categories
			};
			ViewBag.ParrentCategoryId = homeVM.Categories.FirstOrDefault(c=>c.Id==id);
			ViewBag.ThisId = homeVM.Categories.FirstOrDefault(c => c.Id == id).Id;
			return View(homeVM);
		}
		public IActionResult DetailsChild(int id)
		{

			HomeVM homeVM = new HomeVM()
			{
				Products = _context.Products.Include(u => u.Category),
				Categories = _context.Categories
			};
			ViewBag.ThisId = homeVM.Categories.FirstOrDefault(c => c.Id == id).Id;
			return View(homeVM);
		}
		public IActionResult Product(int id)
		{
			List<ShoppingCart> shoppingCartsList = new();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
			{
				shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}

			ProductCardVM productCardVM = new()
			{
				Product = _context.Products.Include(u => u.Category)
				.FirstOrDefault(c => c.Id == id),
				ExistsInCart = false
			};

			foreach(var item in shoppingCartsList)
			{
				if(item.ProductId==id)
				{
					productCardVM.ExistsInCart = true;
				}
			}
			return View(productCardVM);
		}
		[HttpPost,ActionName("Product")]
		public IActionResult ProductPost(int id)
		{
			List<ShoppingCart> shoppingCartsList = new();
			if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null&&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count()>0)
			{
				shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			shoppingCartsList.Add(new ShoppingCart {ProductId =id });
			HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartsList);
			return RedirectToAction(nameof(Product));
		}
		public IActionResult RemoveFromCart(int id)
		{
			List<ShoppingCart> shoppingCartsList = new();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
			{
				shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}

			var itemToRemove = shoppingCartsList.SingleOrDefault(r => r.ProductId == id);
			if(itemToRemove != null)
			{
				shoppingCartsList.Remove(itemToRemove);
			}

			HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartsList);
			return RedirectToAction(nameof(Index));
		}
	}
}