using jwt_auth_manager;
using Microsoft.EntityFrameworkCore;
using vehicle_service_api.DataContexts;
using vehicle_service_api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add db context
builder.Services.AddDbContext<VehicleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuth();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<iVehicleRepo, VehicleRepo>();
builder.Services.AddScoped<iTypeRepo, TypeRepo>();
builder.Services.AddScoped<iCategoryRepo, CategoryRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
