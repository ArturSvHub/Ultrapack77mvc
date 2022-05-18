using Microsoft.AspNetCore.Identity;

namespace Ultrapack77mvc.Models
{
	public class UltrapackUser:IdentityUser
	{
		public string FullName { get; set; }
	}
}
