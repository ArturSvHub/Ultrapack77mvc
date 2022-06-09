using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UpakUtilitiesLibrary;
using UpakUtilitiesLibrary.Utility.Extentions;
using UpakDataAccessLibrary.DataContext;
using UpakModelsLibrary.Models;
using UpakModelsLibrary.Models.ViewModels;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Authorize(Roles = WebConstants.AdminRole)]
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly MssqlContext _context;
		private readonly IWebHostEnvironment _environment;

		public ProductController(MssqlContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		[HttpGet]
		public IActionResult Index()
		{
			IEnumerable<Product> prodList = _context.Products.Include(u => u.Category);

			return View(prodList);
		}
		//GET - upsert
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
			if (id == null)
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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(ProductVM productVM)
		{
			var files = HttpContext.Request.Form.Files;

			if (productVM.Product.Id == 0)
			{
				productVM.Product.ImagePath = files[0].FileName;
				productVM.Product.Image = await files[0].ImageToImageDataAsync();

				await _context.AddAsync(productVM.Product);
			}
			else
			{
				if (files[0] != null)
				{
					productVM.Product.Image = await files[0].ImageToImageDataAsync();
					productVM.Product.ImagePath = files[0].FileName;
				}
				else
				{
					var objFromDb = await _context.Products?.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productVM.Product.Id);
					productVM.Product.Image = objFromDb?.Image;
					productVM.Product.ImagePath = objFromDb.ImagePath;
				}
				_context.Update(productVM.Product);
			}
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}




		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product? product = await _context.Products?.Include(c => c.Category).FirstOrDefaultAsync(u => u.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);

		}


		//POST - delete
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletePost(int? id)
		{

			var obj = await _context.Products.FindAsync(id);
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
