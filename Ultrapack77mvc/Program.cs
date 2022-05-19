using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ultrapack77mvc.DataContext;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UpakGkultraConnextion") ?? throw new InvalidOperationException("Connection string 'MssqlContextConnection' not found.");

builder.Services.AddDbContext<MssqlContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options=>
options.SignIn.RequireConfirmedAccount = false)
	.AddDefaultTokenProviders()
	.AddDefaultUI()
	.AddEntityFrameworkStores<MssqlContext>();
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
	name:"Admin",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
