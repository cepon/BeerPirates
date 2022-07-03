using Product.DTOs;

namespace Product.Services.ProductService
{
  public interface IProductService
  {
    Task<List<ProductDto>> GetProductsAsync();
    Task<List<ProductDto>> GetProductsByIdsAsync(List<int> ids);
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<bool> AddProductAsync(ProductDto product);
    Task<bool> UpdateProductAsync(ProductDto product);
    Task<bool> DeleteProductAsync(int id);
  }
}
