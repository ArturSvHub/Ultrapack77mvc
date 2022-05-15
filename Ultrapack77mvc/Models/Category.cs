using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ultrapack77mvc.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Category Description")]
		public string? Description { get; set; }
		[Required]
		public string Name { get; set; }
		public int? ParentCategoryId { get; set; }
		public Category? ParentCategory { get; set; }
		public List<Category> ChildrenCategories { get; set; } = new();
		public string? ImagePath { get; set; }
		public List<Product>? Products { get; set; }
	}
}
