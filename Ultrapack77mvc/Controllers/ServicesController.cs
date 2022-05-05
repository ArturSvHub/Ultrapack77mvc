using Microsoft.AspNetCore.Mvc;

using Ultrapack77mvc.DataContext;

namespace Ultrapack77mvc.Controllers
{
	public class ServicesController : Controller
	{
		private readonly MssqlContext _context;
		public ServicesController(MssqlContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
