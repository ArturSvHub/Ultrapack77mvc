﻿using Ultrapack77mvc.Models;

namespace Ultrapack77mvc.ViewModels
{
	public class HomeVM
	{
		public IEnumerable<Product> Products { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}
