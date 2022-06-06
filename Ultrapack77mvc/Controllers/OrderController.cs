using Microsoft.AspNetCore.Mvc;

using UpakDataAccessLibrary.DataContext;

namespace Ultrapack77mvc.Controllers
{
	public class OrderController : Controller
	{
		private readonly MssqlContext _context;

		public OrderController(MssqlContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
