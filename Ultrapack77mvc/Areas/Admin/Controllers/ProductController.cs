using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc.Models;
using Ultrapack77mvc.ViewModels;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Authorize(Roles = WebConstants.AdminRole)]
	public class ProductController : Controller
	{
		private readonly MssqlContext _context;
		private readonly IWebHostEnvironment _environment;

		public ProductController(MssqlContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		[Area("Admin")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Product> prodList = await _context.Products.Include(c=>c.Category).ToListAsync();

			return View(prodList);
		}
		//GET - upsert
		[Area("Admin")]
		[HttpGet]
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new ProductVM()
			{
				Product = new Product(),
				CategorySelectedList = _context.Categories
				.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				})
			};
			if (id is null)
			{
				return View(productVM);
			}
			else
			{
				productVM.Product = _context.Products.Find(id);
				if (productVM.Product is null)
				{
					return NotFound();
				}
				return View(productVM);
			}
		}


		//POST - upsert
		[Area("Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM productVM)
		{
			//if (ModelState.IsValid)
			//{
				productVM.Product.CreatedDateTime = DateTime.Now;
				var files = HttpContext.Request.Form.Files;
				string webRootPath = _environment.WebRootPath;

				if (productVM.Product.Id == 0)
				{
					string upload = webRootPath + WebConstants.ProductImagePath;
					string fileName = Guid.NewGuid().ToString();
					string extention = Path.GetExtension(files[0].FileName);

					using (var fileStream = new FileStream(
						Path.Combine(upload, fileName + extention),
						FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					};

					productVM.Product.Image = fileName + extention;

					_context.Add(productVM.Product);
				}
				else
				{
					var objFromDb = _context.Products.AsNoTracking().FirstOrDefault(x =>
					x.Id == productVM.Product.Id);
					if (files.Count > 0)
					{
						string upload = webRootPath + WebConstants.ProductImagePath;
						string fileName = Guid.NewGuid().ToString();
						string extention = Path.GetExtension(files[0].FileName);

						var oldFile = Path.Combine(upload, objFromDb.Image);
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
						productVM.Product.Image = fileName + extention;
					}
					else
					{
						productVM.Product.Image = objFromDb.Image;
					}
					_context.Update(productVM.Product);
				}
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			//}
			//else
			//{
			//	productVM.CategorySelectedList = _context.Categories
			//		.Where(c => c.IsMasterCategory == false)
			//		.Select(i => new SelectListItem
			//		{
			//			Text = i.Name,
			//			Value = i.Id.ToString()
			//		});
			//	return View(productVM);
			//}
		}




		[Area("Admin")]
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null||id==0)
			{
				return NotFound();
			}
			Product? product = _context.Products.Include(u=>u.Category)
				.FirstOrDefault(u=>u.Id==id);
			if(product==null)
			{
				return NotFound();
			}
			return View(product);

		}
		//POST - delete
		[Area("Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{
			
			var obj = _context.Products.Find(id);
			if (obj==null)
			{
				return NotFound();
			}
			string upload = _environment.WebRootPath + WebConstants.ProductImagePath;

			var oldFile = Path.Combine(upload, obj.Image);
			if (System.IO.File.Exists(oldFile))
			{
				System.IO.File.Delete(oldFile);
			}


			_context.Products.Remove(obj);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
