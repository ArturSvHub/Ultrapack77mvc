using Microsoft.AspNetCore.Mvc;

namespace Ultrapack77mvc.Controllers
{
	public class WikiController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
