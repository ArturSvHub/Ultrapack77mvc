using Microsoft.AspNetCore.Mvc;

using Ultrapack77mvc.DataContext;

namespace Ultrapack77mvc.Controllers
{
	public class AboutController : Controller
	{
		private readonly MssqlContext _context;
		public AboutController(MssqlContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
