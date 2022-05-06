using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	public class CategoryController : Controller
	{
		private readonly MssqlContext _context;

		public CategoryController(MssqlContext context)
		{
			_context = context;
		}

		[Area("Admin")]
		public IActionResult Index()
		{
			IEnumerable<Category> catList = _context.Categories;
			return View(catList);
		}
		[Area("Admin")]
		public IActionResult Create()
		{
			return View();
		}
	}
}
