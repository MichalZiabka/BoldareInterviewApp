using BoldareApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddVersioning()
    .AddSwaggerVersioning()
    .AddMemoryCache()
    .AddApiControllers()
    .AddApiServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithVersioning();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();