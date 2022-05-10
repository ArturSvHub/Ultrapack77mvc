using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultrapack77mvc.Models
{ 
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? ShortDesc { get; set; }
		public string? Image { get; set; }
		public string? Barcode { get; set; }
		public int? Article { get; set; }
		public DateTime CreatedDateTime { get; set; }
		[Range(1, 5000000)]
		public decimal? PurchasePrice { get; set; }
		[Range(1, 50000000)]
		public decimal? RetailPrice { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
	}
}
