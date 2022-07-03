using Microsoft.EntityFrameworkCore;
using Product.Database;
using Product.Database.Context;
using Product.DTOs;
using Product.Exceptions;

namespace Product.Repositories.ProductRepository
{
  public class ProductRepository : IProductRepository
  {
    private readonly DataContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(DataContext context, ILogger<ProductRepository> logger)
    {
      _context = context;
      _logger = logger;
    }
    public async Task AddProductAsync(ProductModel product)
    {
      _context.Products.Add(product);
      await _context.SaveChangesAsync();
    }

    public async Task AddProductTagAsync(ProductTags productTag)
    {
      _context.ProductTags.Add(productTag);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
      ProductModel product = await _context.Products
        .Where(p => p.Id == id && p.Active == Database.Enums.ActiveEnum.Active)
        .FirstOrDefaultAsync();

      if (product == null)
      {
        _logger.LogError($"Product with id={id} does not exists.");
        throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, "Product does not exist");        
      }

      product.Active = Database.Enums.ActiveEnum.Deleted;
      product.DateUpdate = DateTime.UtcNow;
      await _context.SaveChangesAsync();
      _logger.LogInformation($"Product with id={id} is deleted.");
    }

    public async Task<ProductModel> GetProductByIdAsync(int id)
    {
      ProductModel product = await _context.Products
        .Where(p => p.Id == id && p.Active == Database.Enums.ActiveEnum.Active)
        .Include(p => p.ProductTags.Where(pt => pt.Active == Database.Enums.ActiveEnum.Active))
          .ThenInclude(pt => pt.Tag)
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .FirstOrDefaultAsync();

      return product;
    }

    public async Task<List<ProductModel>> GetProductsAsync()
    {
      List<ProductModel> products = await _context.Products
        .Where(p => p.Active == Database.Enums.ActiveEnum.Active)
        .Include(p => p.ProductTags.Where(pt => pt.Active == Database.Enums.ActiveEnum.Active))
          .ThenInclude(pt => pt.Tag)
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .ToListAsync();

      return products;
    }

    public async Task<ProductTags> GetProductTagAsync(int productId, int tagId)
    {
      ProductTags productTags = await _context.ProductTags
        .Where(pt => pt.ProductId == productId && pt.TagId == tagId && pt.Active == Database.Enums.ActiveEnum.Active)
        .FirstOrDefaultAsync();

      return productTags;
    }

    public async Task DeleteProductTagsAsync(int productId, List<int> tagIds)
    {
      List<ProductTags> productTagsToDelete = await _context.ProductTags
         .Where(pt => pt.ProductId == productId && !tagIds.Contains(pt.TagId) && pt.Active == Database.Enums.ActiveEnum.Active)
         .ToListAsync();

      productTagsToDelete.ForEach(pt =>
      {
        pt.DateUpdate = DateTime.UtcNow;
        pt.Active = Database.Enums.ActiveEnum.Deleted;
      });
      await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(ProductModel product)
    {
      ProductModel productModel = await GetProductByIdAsync(product.Id);
      product.DateUpdate = DateTime.UtcNow;
      _context.Entry(productModel).CurrentValues.SetValues(product);
      await _context.SaveChangesAsync();
    }

    public async Task<List<ProductModel>> GetProductsByIdsAsync(List<int> ids)
    {
      List<ProductModel> products = await _context.Products
        .Where(p => ids.Contains(p.Id) && p.Active == Database.Enums.ActiveEnum.Active)
        .Include(p => p.ProductTags.Where(pt => pt.Active == Database.Enums.ActiveEnum.Active))
          .ThenInclude(pt => pt.Tag)
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .ToListAsync();

      return products;
    }
  }
}
