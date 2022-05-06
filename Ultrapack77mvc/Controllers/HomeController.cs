using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc.Models;

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
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
	}
}