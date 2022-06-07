using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ultrapack77mvc.DataContext;
using Ultrapack77mvc;

namespace Ultrapack77mvc.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = WebConstants.AdminRole)]
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
