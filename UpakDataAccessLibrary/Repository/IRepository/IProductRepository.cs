
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using UpakModelsLibrary.Models;

namespace UpakDataAccessLibrary.Repository.IRepository
{
	public interface IProductRepository:IRepository<Product>
	{
		void Update(Product product);

		IEnumerable<SelectListItem> GetDropdownList();
	}

}
