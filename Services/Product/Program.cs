using Microsoft.EntityFrameworkCore;
using Product.Database.Context;
using Product.Exceptions;
using Product.Repositories.BrandRepository;
using Product.Repositories.CategoryRepository;
using Product.Repositories.ProductRepository;
using Product.Repositories.TagRepository;
using Product.Services.ProductService;

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

builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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
