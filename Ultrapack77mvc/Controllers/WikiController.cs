using Microsoft.AspNetCore.Mvc;

using Ultrapack77mvc.DataContext;

namespace Ultrapack77mvc.Controllers
{
	public class WikiController : Controller
	{
		private readonly MssqlContext _context;
		public WikiController(MssqlContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
