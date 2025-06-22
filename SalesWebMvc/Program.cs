using System.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using SalesWebMvc.Services;



var builder = WebApplication.CreateBuilder(args);
var serverVersion = new MySqlServerVersion(new Version(8,4));


builder.Services.AddDbContext<SalesWebMvcContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMvcContext") 
    ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found."), serverVersion));

//Add services to the container.
builder.Services.AddControllersWithViews();

//SeedingService is used to populate the database with initial data.
builder.Services.AddScoped<SeedingService>();
var seedingService = new SeedingService(new SalesWebMvcContext(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<SalesWebMvcContext>>()));

//Add other services as needed
builder.Services.AddScoped<SellerService>();

var app = builder.Build();

seedingService.Seed(); // Seed the database with initial data


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
