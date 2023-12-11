using System.Text.Json.Serialization;
using api;
using service;
using infrastructure;
using infrastructure.Reposotories;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<TaxaRepository>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<MailService>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();
//Enable sessions
builder.Services.AddSession(options =>
{
    //Session expired after 4 hours
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

//The project can access this from everywhere.
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

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseSession();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();