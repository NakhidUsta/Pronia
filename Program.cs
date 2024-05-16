using Microsoft.EntityFrameworkCore;
using Pronia.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute("admin", "{area:exists}/{controller=dashboard}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
app.Run();
