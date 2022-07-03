using Microsoft.EntityFrameworkCore;
using Product.Database;
using Product.Database.Context;

namespace Product.Repositories.CategoryRepository
{
  public class CategoryRepository : ICategoryRepository
  {
    private readonly DataContext _context;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(DataContext context, ILogger<CategoryRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task AddCategoryAsync(Category category)
    {
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();
      _logger.LogInformation($"Created category {category.Name}");
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
      Category category = await _context.Categories
         .Where(c => c.Name == name && c.Active == Database.Enums.ActiveEnum.Active)
         .FirstOrDefaultAsync();

      return category;
    }
  }
}
