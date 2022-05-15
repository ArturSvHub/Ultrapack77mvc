using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc.Models;
using Ultrapack77mvc.ViewModels;

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

			ProductCardVM productCardVM = new()
			{
				Product = _context.Products.Include(u => u.Category).FirstOrDefault(c => c.Id == id)
			};
			return View(productCardVM);
		}
	}
}