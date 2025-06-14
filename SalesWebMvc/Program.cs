using System.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;


var builder = WebApplication.CreateBuilder(args);
var serverVersion = new MySqlServerVersion(new Version(8,4));


builder.Services.AddDbContext<SalesWebMvcContext>(options =>
//options.UseMySql(serverVersion, "SalesWebMvcContext")
options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMvcContext") 
    ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found."), serverVersion));

//Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
