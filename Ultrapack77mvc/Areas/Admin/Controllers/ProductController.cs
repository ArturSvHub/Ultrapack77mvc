using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UpakUtilitiesLibrary;
using UpakDataAccessLibrary.DataContext;
using UpakModelsLibrary.Models;
using UpakModelsLibrary.Models.ViewModels;
using UpakDataAccessLibrary.Repository.IRepository;

namespace Ultrapack77mvc.Areas.Admin.Controllers
{
	[Authorize(Roles = WebConstants.AdminRole)]
	public class ProductController : Controller
	{
		private readonly IProductRepository _prodContext;
		private readonly IWebHostEnvironment _environment;

		public ProductController(IProductRepository context, IWebHostEnvironment environment)
		{
			_prodContext = context;
			_environment = environment;
		}

		[Area("Admin")]
		[HttpGet]
		public IActionResult Index()
		{
			IEnumerable<Product> prodList = _prodContext
				.GetAll(includeProperties:"Category");

			return View(prodList);
		}
		//GET - upsert
		[Area("Admin")]
		[HttpGet]
		public async Task<IActionResult> Upsert(int? id)
		{
			ProductVM productVM = new ProductVM()
			{
				Product = new Product(),
				CategorySelectedList = _prodContext.GetDropdownList()
			};
			if (id is null)
			{
				return View(productVM);
			}
			else
			{
				productVM.Product =await _prodContext.FindAsync(id.GetValueOrDefault());
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
		public async Task<IActionResult> Upsert(ProductVM productVM)
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

					await _prodContext.AddAsync(productVM.Product);
				}
				else
				{
					var objFromDb = _prodContext.FirstOrDefault(x =>
					x.Id == productVM.Product.Id,isTracking:false);
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
					_prodContext.Update(productVM.Product);
				}
				await _prodContext.SaveAsync();
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
			Product? product = _prodContext.FirstOrDefault(u=>u.Id==id,includeProperties: "Category");
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
		public async Task<IActionResult> DeletePost(int? id)
		{
			
			var obj =await _prodContext.FindAsync(id.GetValueOrDefault());
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


			_prodContext.Remove(obj);
			await _prodContext.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
