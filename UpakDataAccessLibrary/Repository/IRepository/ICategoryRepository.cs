﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using UpakModelsLibrary.Models;

namespace UpakDataAccessLibrary.Repository.IRepository
{
	public interface ICategoryRepository:IRepository<Category>
	{
		void Update(Category category);
	}
}