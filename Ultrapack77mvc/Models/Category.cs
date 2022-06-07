using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ultrapack77mvc.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Описаниие категории")]
		public string? Description { get; set; }
		[Required]
		[DisplayName("Название категории")]
		public string Name { get; set; }
		public string? ImagePath { get; set; }
		public List<Product>? Products { get; set; }
	}
}
