using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using jwt_auth_manager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();
builder.Services.AddCustomJwtAuth();
var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
