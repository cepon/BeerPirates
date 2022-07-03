using BeerRecommendations.Database.Context;
using BeerRecommendations.Exceptions;
using BeerRecommendations.Repositories;
using BeerRecommendations.Services.BeerRecommendationsService;
using BeerRecommendations.Services.Consumed.ProductService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers()
  .AddJsonOptions(jsonOptions =>
  {
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
  })
    .ConfigureApiBehaviorOptions(options =>
    {
      options.InvalidModelStateResponseFactory = context =>
      {
        throw new ErrorException(context.ModelState);
      };
    });

builder.Services.AddScoped<IBeerRecommendationsService, BeerRecommendationsService>();
builder.Services.AddScoped<IBeerRecommendationsRepository, BeerRecommendationsRepository>();
// TODO: check if needed.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpClient<IProductService, ProductService>(u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductService"]));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// EF Core automatic migration to database.
using (var scope = app.Services.CreateScope())
{
  var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
  dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseExceptionHandler("/api/error");

app.UseAuthorization();

app.MapControllers();

app.Run();
