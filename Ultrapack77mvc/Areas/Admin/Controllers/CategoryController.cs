using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpakDataAccessLibrary.DataContext;
using UpakModelsLibrary.Models;
using UpakModelsLibrary.Models.ViewModels;
using UpakUtilitiesLibrary.Utility.Extentions;
using UpakUtilitiesLibrary;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = WebConstants.AdminRole)]
	public class CategoryController : Controller
	{
		
		private readonly MssqlContext? _context;

		private readonly IWebHostEnvironment? _environment;

		public CategoryController(MssqlContext catContext, IWebHostEnvironment environment)
		{
			_context = catContext;
			_environment = environment;
		}



		
		public async Task<IActionResult> Index()
		{
			List<Category> catList =await _context.Categories?.ToListAsync();
			return View(catList);
		}
		//GET - Create
		
		public IActionResult Create()
		{
			return View();
		}
		//POST-Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			var files = HttpContext.Request.Form.Files;

			
			category.Image = await files[0].ImageToImageDataAsync();
			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
			
		}

		//GET-EDIT

		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id==null|| id == 0)
			{
				return NotFound();
			}
			else
			{
				var obj =await _context.Categories.FindAsync(id);
				if (obj == null)
				{
					return NotFound();
				}
				return View(obj);
			}
		}

		//POST-Edit

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category category)
		{
			var files = HttpContext.Request.Form.Files;
			
			if (files.Count > 0)
			{
				category.Image = await files[0].ImageToImageDataAsync();
			}
			else
			{
				var objFromDb = await _context.Categories?.AsNoTracking().FirstOrDefaultAsync(x =>
			x.Id == category.Id);
				category.Image = objFromDb.Image;
			}
			_context.Categories.Update(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}




		//GET-Delete

		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? category =await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);

		}
		//POST - delete

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletePost(int? id)
		{

			var obj =await _context.Categories.FindAsync(id);
			if (obj == null)
			{
				return NotFound();
			}
			_context.Remove(obj);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}

