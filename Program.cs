using Microsoft.EntityFrameworkCore;
using Pronia.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer("server=LAPTOP-KU5O0OFQ;database=Pronia;trusted_connection=true;integrated security=true;encrypt=false"));
var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");

app.Run();
