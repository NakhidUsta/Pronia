using Microsoft.EntityFrameworkCore;
using Pronia.Context;
using Pronia.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options=>options.IdleTimeout=TimeSpan.FromSeconds(10));
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
builder.Services.AddScoped<LayoutService>();

var app = builder.Build();
app.UseSession();  
app.UseStaticFiles();
app.MapControllerRoute("admin", "{area:exists}/{controller=dashboard}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
app.Run();
