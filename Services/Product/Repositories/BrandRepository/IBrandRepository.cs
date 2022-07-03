using Product.Database;

namespace Product.Repositories.BrandRepository
{
  public interface IBrandRepository
  {
    Task<Brand> GetBrandByNameAsync(string name);
    Task AddBrandAsync(Brand brand);
  }
}
