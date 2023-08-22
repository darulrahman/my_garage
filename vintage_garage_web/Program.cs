using vintage_garage_web.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using vintage_garage_web.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<vintage_garage_webContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("vintage_garage_webContext") ?? throw new InvalidOperationException("Connection string 'vintage_garage_webContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddScoped<iGarageRepo, GarageRepo>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "Login";
    options.SlidingExpiration = true;
});
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

app.UseSession();
//app.UseMvc();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehicle}/{action=Index}/{id?}");

app.Run();
