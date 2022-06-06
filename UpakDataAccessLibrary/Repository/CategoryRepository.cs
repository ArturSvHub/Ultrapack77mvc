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
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private readonly MssqlContext _context;

		public CategoryRepository(MssqlContext context):base(context)
		{
			_context = context;
		}

		public void Update(Category category)
		{
			var categoryFromDb = _context.Categories.FirstOrDefault(c=>c.Id== category.Id);
			if(categoryFromDb!=null)
			{
				categoryFromDb.Name = category.Name;
			}
		}

	}
}
