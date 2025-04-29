using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Areas.Identity.Data;
using MyWebsite.Data;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyWebsiteUserContextConnection") ?? throw new InvalidOperationException("Connection string 'MyWebsiteUserContextConnection' not found.");

builder.Services.AddDbContext<MyWebsiteUserContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MyWebsiteUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MyWebsiteUserContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyCommentsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCommentsContext") ?? throw new InvalidOperationException("Connection string 'MyCommentsContext' not found.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
