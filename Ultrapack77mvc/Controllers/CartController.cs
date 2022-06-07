using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Text;

using Ultrapack77mvc.DataContext;
using Ultrapack77mvc;
using Ultrapack77mvc.Models;
using Ultrapack77mvc.Utility.Extentions;
using Ultrapack77mvc.Models.ViewModels;

namespace Ultrapack77mvc.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly MssqlContext _context;
		private readonly IWebHostEnvironment _environment;
		private readonly IEmailSender _emailSender;
		[BindProperty]
		public ProductUserVM ProductUserVM { get; set; }

		public CartController(MssqlContext context,IWebHostEnvironment environment,IEmailSender emailSender)
		{
			_context = context;
			_environment=environment;
			_emailSender=emailSender;
		}

		public IActionResult Index()
		{
			List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
			if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null&&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count()>0)
			{
				shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			List<int> prodInCart = shoppingCartList.Select(i=>i.ProductId).ToList();
			List<Product> prodList = _context.Products.Where(u => prodInCart.Contains(u.Id)).ToList();

			return View(prodList);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Index")]
		public IActionResult IndexPost()
		{
			return RedirectToAction(nameof(Summary));
		}

        public IActionResult Summary()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			//var userId = User.FindFirstValue(ClaimTypes.Name);

			List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
			{
				shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
			List<Product> prodList = _context.Products.Where(u => prodInCart.Contains(u.Id)).ToList();

			ProductUserVM = new()
			{
				ApplicationUser = _context.UltrapackUsers.FirstOrDefault(u => u.Id == claim.Value),
				ProductList = prodList
			};
			return View(ProductUserVM); 
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Summary")]
		public async Task<IActionResult> SummaryPost(ProductUserVM productUserVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


			var PathToTemplate = _environment.WebRootPath + Path.DirectorySeparatorChar.ToString() +
				"templates" + Path.DirectorySeparatorChar.ToString() +
				"inquiry.html";
			var subject = "Новый заказ";
			string HtmlBody = "";
			using(StreamReader sr = System.IO.File.OpenText(PathToTemplate))
			{
				HtmlBody = sr.ReadToEnd();
			}
			StringBuilder productListSB = new StringBuilder();
			foreach(var item in productUserVM.ProductList)
			{
				productListSB.Append($" - {item.Name} <span style='font-size:14px;' (ID: {item.Id})</span></br>");
			}
			string messageBody = string.Format(HtmlBody,
				ProductUserVM.ApplicationUser.FullName,
				ProductUserVM.ApplicationUser.Email,
				ProductUserVM.ApplicationUser.PhoneNumber,
				productListSB.ToString()
				);

			await _emailSender.SendEmailAsync(WebConstants.EmailForEnquires,subject,messageBody);

			OrderHeader orderHeader = new OrderHeader()
			{
				UltrapackUserId = claim.Value,
				FullName = ProductUserVM.ApplicationUser.FullName,
				Email = ProductUserVM.ApplicationUser.Email,
				PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
				OrderDate = DateTime.Now
			};

			await _context.AddAsync(orderHeader);
			await _context.SaveChangesAsync();

			foreach (var prod in ProductUserVM.ProductList)
			{
				OrderDetails orderDetails = new OrderDetails()
				{
					OrderHeaderId = orderHeader.Id,
					ProductId = prod.Id
				};
				await _context.AddAsync(orderDetails);
			}
			await _context.SaveChangesAsync();


			return RedirectToAction(nameof(InquiryConfirm));
		}
		public IActionResult InquiryConfirm()
		{
			HttpContext.Session.Clear();
			return View();
		}
		public IActionResult Remove(int id)
		{
			List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
			{
				shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
			}
			shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
			HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

			return RedirectToAction(nameof(Index));
		}
	}
}
