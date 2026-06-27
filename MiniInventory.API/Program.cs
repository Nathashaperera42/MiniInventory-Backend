using System.Text.Json.Serialization;
using MiniInventory.API.Middleware;
using MiniInventory.Application.DependencyInjection;
using MiniInventory.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ----- Controllers + JSON options -----
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ----- Swagger / OpenAPI -----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mini Inventory Management API",
        Version = "v1",
        Description = "Hardware & Building Supplies inventory system - Clean Architecture demo."
    });
});

// ----- CORS for the Flutter frontend -----
const string CorsPolicy = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ----- Clean Architecture layer registrations -----
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// ----- Pipeline -----
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
