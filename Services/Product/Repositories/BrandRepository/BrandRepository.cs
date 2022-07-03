using Microsoft.EntityFrameworkCore;
using Product.Database;
using Product.Database.Context;

namespace Product.Repositories.BrandRepository
{
  public class BrandRepository : IBrandRepository
  {
    private readonly DataContext _context;
    private readonly ILogger<BrandRepository> _logger;

    public BrandRepository(DataContext context, ILogger<BrandRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task AddBrandAsync(Brand brand)
    {
      _context.Brands.Add(brand);
      await _context.SaveChangesAsync();
      _logger.LogInformation($"Created brand {brand.Name}");
    }

    public async Task<Brand> GetBrandByNameAsync(string name)
    {
      Brand brand = await _context.Brands
         .Where(b => b.Name == name && b.Active == Database.Enums.ActiveEnum.Active)
         .FirstOrDefaultAsync();

      return brand;
    }
  }
}
