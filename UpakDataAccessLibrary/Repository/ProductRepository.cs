using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using UpakDataAccessLibrary.DataContext;
using UpakDataAccessLibrary.Repository.IRepository;

using UpakModelsLibrary.Models;

namespace UpakDataAccessLibrary.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly MssqlContext _context;

		public ProductRepository(MssqlContext context):base(context)
		{
			_context = context;
		}

		public IEnumerable<SelectListItem> GetDropdownList()
		{
			return _context.Categories
				.Where(c => c.ParentCategory != null)
				.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});
		}

		public void Update(Product product)
		{
			_context.Products.Update(product);
		}

	}
}
