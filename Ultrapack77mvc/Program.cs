using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using UpakDataAccessLibrary.DataContext;
using UpakDataAccessLibrary.Repository;
using UpakDataAccessLibrary.Repository.IRepository;

using UpakUtilitiesLibrary.Utility.EmailServices;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UpakGkultraConnextion") ?? throw new InvalidOperationException("Connection string 'MssqlContextConnection' not found.");

builder.Services.AddDbContext<MssqlContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
	options.SignIn.RequireConfirmedAccount = true;
	options.Password.RequiredLength = 5;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireDigit = false;
})
	.AddDefaultTokenProviders()
	.AddDefaultUI()
	.AddEntityFrameworkStores<MssqlContext>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(opts =>
{
	opts.IdleTimeout = TimeSpan.FromMinutes(10);
	opts.Cookie.HttpOnly = true;
	opts.Cookie.IsEssential = true;
});
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
//app.MapAreaControllerRoute(
//	name:"Admin",
//	areaName:"Admin",
//	pattern: "admin/{controller=Home}/{action=Index}/{id?}"
//	);
app.MapControllerRoute(
	name: "Admin",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
