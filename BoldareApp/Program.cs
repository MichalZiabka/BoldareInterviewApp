using BoldareApp.Infrastructure.Configuration;
using BoldareApp.Infrastructure.Extensions;
using BoldareApp.Infrastructure.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services
    .AddVersioning()
    .AddSwaggerVersioning()
    .AddMemoryCache()
    .AddApiServices()
    .AddJwtAutorization(builder.Configuration)
    .AddDbCache(builder.Configuration)
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithVersioning();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); ;
app.MapControllers();
app.UseDbCache();
app.Run();