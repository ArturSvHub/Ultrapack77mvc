using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpakDataAccessLibrary.DataContext;
using UpakModelsLibrary.Models;
using UpakModelsLibrary.Models.ViewModels;

using UpakUtilitiesLibrary;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = WebConstants.AdminRole)]
	public class CategoryController : Controller
	{
		
		private readonly MssqlContext _context;

		private readonly IWebHostEnvironment _environment;

		public CategoryController(MssqlContext catContext, IWebHostEnvironment environment)
		{
			_context = catContext;
			_environment = environment;
		}



		
		public IActionResult Index()
		{
			IEnumerable<Category> catList = _context.Categories;
			return View(catList);
		}
		//GET - Create
		
		public IActionResult Create()
		{
			CategoryVM categoryVM = new()
			{
				Category = new Category(),
				CategoriesForSelect = _context.Categories.ToList()
			};
			return View(categoryVM);
		}
		//POST-Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CategoryVM categoryVM)
		{
			//if (ModelState.IsValid)
			//{
			var files = HttpContext.Request.Form.Files;
			string webRootPath = _environment.WebRootPath;

			string upload = webRootPath + WebConstants.CategoryImagePath;
			string fileName = Guid.NewGuid().ToString();
			string extention = Path.GetExtension(files[0].FileName);

			using (var fileStream = new FileStream(
				Path.Combine(upload, fileName + extention),
				FileMode.Create))
			{
				files[0].CopyTo(fileStream);
			}

			categoryVM.Category.ImagePath = fileName + extention;

			_context.Add(categoryVM.Category);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
			
		}

		//GET-EDIT

		[HttpGet]
		public IActionResult Edit(int id)
		{
			CategoryVM categoryVM = new CategoryVM()
			{
				Category = new Category(),
				CategoriesForSelect =_context.Categories.ToList()
			};
			if (id == 0)
			{
				return View(categoryVM);
			}
			else
			{
				categoryVM.Category =_context.Categories.FirstOrDefault(x=>x.Id==id);
				if (categoryVM.Category is null)
				{
					return NotFound();
				}
				return View(categoryVM);
			}
		}

		//POST-Edit

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CategoryVM categoryVM)
		{
			var files = HttpContext.Request.Form.Files;
			string webRootPath = _environment.WebRootPath;

			var objFromDb = _context.Categories.AsNoTracking().FirstOrDefault(x =>
			x.Id == categoryVM.Category.Id);
			if (files.Count > 0)
			{
				string upload = webRootPath + WebConstants.CategoryImagePath;
				string fileName = Guid.NewGuid().ToString();
				string extention = Path.GetExtension(files[0].FileName);

				var oldFile = Path.Combine(upload, objFromDb.ImagePath);
				if (System.IO.File.Exists(oldFile))
				{
					System.IO.File.Delete(oldFile);
				}

				using (var fileStream = new FileStream(
				Path.Combine(upload, fileName + extention),
				FileMode.Create))
				{
					files[0].CopyTo(fileStream);
				};
				categoryVM.Category.ImagePath = fileName + extention;
			}
			else
			{
				categoryVM.Category.ImagePath = objFromDb.ImagePath;
			}
			_context.Update(categoryVM.Category);
			_context.SaveChanges();
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
			Category? category =await _context.Categories.FindAsync(id.GetValueOrDefault());
			if (category == null)
			{
				return NotFound();
			}
			return View(category);

		}
		//POST - delete

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletePost(int? id)
		{

			var obj =await _context.Categories.FindAsync(id.GetValueOrDefault());
			if (obj == null)
			{
				return NotFound();
			}
			string upload = _environment.WebRootPath + WebConstants.CategoryImagePath;

			var oldFile = Path.Combine(upload, obj.ImagePath);
			if (System.IO.File.Exists(oldFile))
			{
				System.IO.File.Delete(oldFile);
			}


			_context.Remove(obj);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}

