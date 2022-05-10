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
		public bool? IsMasterCategory { get; set; } = false;
		[DisplayName("Name of Master Category")]
		public string? MasterCategoryName { get; set; }
		public string? ImagePath { get; set; }
		public List<Product>? Products { get; set; }
	}
}
