using Product.Database;
using Product.DTOs;

namespace Product.Repositories.ProductRepository
{
  public interface IProductRepository
  {
    Task<List<ProductModel>> GetProductsAsync();
    Task<List<ProductModel>> GetProductsByIdsAsync(List<int> ids);
    Task<ProductModel> GetProductByIdAsync(int id);
    Task AddProductAsync(ProductModel product);
    Task UpdateProductAsync(ProductModel product);
    Task DeleteProductAsync(int id);
    Task<ProductTags> GetProductTagAsync(int productId, int tagId);
    Task AddProductTagAsync(ProductTags productTag);
    Task DeleteProductTagsAsync(int productId, List<int> tagIds);
  }
}
