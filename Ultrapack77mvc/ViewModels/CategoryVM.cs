﻿using Microsoft.AspNetCore.Mvc.Rendering;

using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.ViewModels
{
	public class CategoryVM
	{
		public Category Category { get; set; }
		public IEnumerable<Category> CategoriesForSelect { get; set; }
	}
}