using BeerRecommendations.DTOs;

namespace BeerRecommendations.Services.Consumed.ProductService
{
  public interface IProductService
  {
    Task<ProductDto> GetProduct(int id);
    Task<List<ProductDto>> GetProducts(List<int> ids);
  }
}
