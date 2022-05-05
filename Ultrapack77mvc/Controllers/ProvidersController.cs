using Microsoft.AspNetCore.Mvc;

using Ultrapack77mvc.DataContext;

namespace Ultrapack77mvc.Controllers
{
	public class ProvidersController : Controller
	{
		private readonly MssqlContext _context;
		public ProvidersController(MssqlContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
