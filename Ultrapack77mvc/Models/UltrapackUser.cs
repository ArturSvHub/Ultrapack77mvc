using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ultrapack77mvc.Models
{
	public class UltrapackUser:IdentityUser
	{
		public string FullName { get; set; }
	}
}
