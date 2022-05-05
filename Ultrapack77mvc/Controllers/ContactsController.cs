using Microsoft.AspNetCore.Mvc;

using Ultrapack77mvc.DataContext;

namespace Ultrapack77mvc.Controllers
{
	public class ContactsController : Controller
	{
		private readonly MssqlContext _context;
		public ContactsController(MssqlContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
