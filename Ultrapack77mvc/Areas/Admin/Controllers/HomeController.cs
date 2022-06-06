using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpakUtilitiesLibrary;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Authorize(Roles = WebConstants.AdminRole)]
	[Area("Admin")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return RedirectToAction(nameof(Index),"Category");
		}
	}
}
