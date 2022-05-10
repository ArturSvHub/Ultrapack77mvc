using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
		public async Task<IActionResult> Index()
		{
			IEnumerable<Category> catList = await _context.Categories.ToListAsync();
			return View(catList);
		}
		//GET - Create
		[Area("Admin")]
		public IActionResult Create()
		{
			List<string> catName = new List<string>();
			ViewBag.CatList = _context.Categories.Where(c => c.IsMasterCategory == true)
				.Select(c => c.Name).ToList();
			return View();
		}

		[Area("Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category obj)
		{
			if (ModelState.IsValid)
			{
				await _context.AddAsync(obj);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(obj);
		}
		//GET-EDIT
		[Area("Admin")]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			
			var obj = await _context.Categories.FindAsync(id);
			if(obj == null)
			{
				return NotFound();
			}

			IEnumerable<SelectListItem> CategoryDropDown = _context.Categories.Where(c => c.IsMasterCategory == true).Select(i =>
			new SelectListItem
			{
				Text = i.Name,
				Value = i.Id.ToString()
			});
			if (obj.MasterCategoryName is not null)
			{
				string ActiveItem = obj.MasterCategoryName;
				foreach (var item in CategoryDropDown)
				{
					if (item.Value == ActiveItem)
					{
						item.Selected = true;
					}
				}
			}
			ViewBag.CategoryName = obj.Name;
			ViewBag.CategoryDescription=obj.Description;
			ViewBag.CategoryDropDown = CategoryDropDown;
			return View();
		}
		[Area("Admin")]
		[HttpPost]
		public async Task<IActionResult> Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
				_context.Update(obj);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(obj);
		}
		//GET-Delete
		[Area("Admin")]
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var obj = await _context.Categories.FindAsync(id);
			if (obj == null)
			{
				return NotFound();
			}
			return View(obj);
		}
		//POST-Delete
		[Area("Admin")]
		[HttpPost]
		public async Task<IActionResult> DeletePost(int? id)
		{
			var obj = await _context.Categories.FindAsync(id);
			if (obj == null)
			{
				return NotFound();
			}
				_context.Categories.Remove(obj);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
		}
	}
}
