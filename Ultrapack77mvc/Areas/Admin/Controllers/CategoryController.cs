using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc.Models;
using Ultrapack77mvc.ViewModels;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = WebConstants.AdminRole)]
	public class CategoryController : Controller
	{
		
		private readonly MssqlContext _context;
		IWebHostEnvironment _environment;

		public CategoryController(MssqlContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}





		
		public async Task<IActionResult> Index()
		{
			IEnumerable<Category> catList = await _context.Categories.ToListAsync();
			return View(catList);
		}
		//GET - Create
		
		public IActionResult Create()
		{
			CategoryVM categoryVM = new()
			{
				Category = new Category(),
				CategoriesForSelect = _context.Categories.Where(c=>c.ParentCategory==null).ToList()
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
			};

			categoryVM.Category.ImagePath = fileName + extention;

			_context.Add(categoryVM.Category);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
			//}
			//else
			//{
			//	categoryVM.CategorySelectedList = _context.Categories
			//		.Where(c => c.IsMasterCategory == true)
			//		.Select(i => new SelectListItem
			//		{
			//			Text = i.Name,
			//			Value = i.Id.ToString()
			//		});
			//	return View(categoryVM);
			//}
		}

		//GET-EDIT

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			CategoryVM categoryVM = new CategoryVM()
			{
				Category = new Category(),
				CategoriesForSelect = _context.Categories.Where(c => c.ParentCategory == null).ToList()
			};
			if (id is null)
			{
				return View(categoryVM);
			}
			else
			{
				categoryVM.Category = _context.Categories.Find(id);
				if (categoryVM.Category is null)
				{
					return NotFound();
				}
				return View(categoryVM);
			}
		}

		//POST-Edit

		[HttpPost]
		public IActionResult Edit(CategoryVM categoryVM)
		{
			//if (ModelState.IsValid)
			//{
			var files = HttpContext.Request.Form.Files;
			string webRootPath = _environment.WebRootPath;

			var objFromDb = _context.Categories.AsNoTracking().FirstOrDefault(x =>
			x.Id == categoryVM.Category.Id);
			//ViewBag.MasterCatName = _context.Categories
			//.FirstOrDefault(c => c.Id == objFromDb.MasterCategoryId).Name;  
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
			//}
			//else
			//{
			//	categoryVM.CategorySelectedList = _context.Categories
			//		.Where(c => c.IsMasterCategory == true)
			//		.Select(i => new SelectListItem
			//		{
			//			Text = i.Name,
			//			Value = i.Id.ToString()
			//		});
			//	return View(categoryVM);
		}




		//GET-Delete

		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? category = _context.Categories
				.FirstOrDefault(u => u.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);

		}
		//POST - delete

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{

			var obj = _context.Categories.Find(id);
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


			_context.Categories.Remove(obj);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}

