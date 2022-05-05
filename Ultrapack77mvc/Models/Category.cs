using System.ComponentModel.DataAnnotations;

namespace Ultrapack77mvc.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
		[Required]
		public string Name { get; set; }
		public string SubCategoryName { get; set; }
		public byte[] Image { get; set; }
		public List<Product> Products { get; set; }
	}
}
