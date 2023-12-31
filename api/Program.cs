using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using System.Net.Http;
using System.Text.Json.Serialization;
using api;
using api.middleware;
using api.Middleware;
using service;
using infrastructure;
using infrastructure.Reposotories;

var builder = WebApplication.CreateBuilder(args);

// Getting swagger to work with bearer tokens
builder.Services.AddSwaggerGenWithBearerJWT();

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}

builder.Services.AddScoped<TaxaService>();
builder.Services.AddScoped<MapsRepository>();
builder.Services.AddScoped<MapsService>();
builder.Services.AddScoped<TaxaRepository>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<MailService>();
builder.Services.AddJwtService();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// The project can access this from everywhere.
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PasswordHashRepository>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This makes the headers secure, but it cannot talk with the frontend if enabled
// Security policies with the web browser, based on name
app.UseSecurityHeaders();

// For allowing cross-site scripting and allowing the API to talk with frontend
var allowedOrigins = new[]
{
    "http://localhost:4200",
    "https://taxapp-707f6.web.app",
};

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});


// Adding middleware for handling JWT Bearer tokens
app.UseMiddleware<JwtBearerHandler>();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();
app.Run();
