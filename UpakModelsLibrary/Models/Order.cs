using System.ComponentModel.DataAnnotations;

namespace UpakModelsLibrary.Models
{
	public class Order
    {
        [Key]
        public int Id { get; set; }
        public List<Product> Products { get; set; }
        public string? Description { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
		public List<ProductOrder>? ProductOrders { get; set; }
	}
}
