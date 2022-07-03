using Product.Database;

namespace Product.Repositories.CategoryRepository
{
  public interface ICategoryRepository
  {
    Task<Category> GetCategoryByNameAsync(string name);
    Task AddCategoryAsync(Category category);
  }
}
