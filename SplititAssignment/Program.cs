using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SplititAssignment;
using SplititAssignment.Data;
using SplititAssignment.Helper;
using SplititAssignment.Interfaces;
using SplititAssignment.Repository;
using SplititAssignment.Scrapers;
using SplititAssignment.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Splitit.Job.Assignment\r\n", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<IScraper, IMDbScraper>();
builder.Services.AddScoped<IScraper, PinkvillaScraper>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IScraperService, ScraperService>();
builder.Services.AddScoped<CustomResponseFilterAttribute>();

builder.Services.AddDbContext<DataContext>(options =>
        options.UseInMemoryDatabase("ActorsDB"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var scraperService = scope.ServiceProvider.GetRequiredService<IScraperService>();
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.EnsureCreated();
    await scraperService.ScrapeActorsAsync();
}

app.Run();
