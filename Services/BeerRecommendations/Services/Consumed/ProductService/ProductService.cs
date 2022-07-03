using BeerRecommendations.DTOs;
using BeerRecommendations.Exceptions;
using System.Text.Json;

namespace BeerRecommendations.Services.Consumed.ProductService
{
  public class ProductService : IProductService
  {
    private readonly HttpClient client;

    public ProductService(HttpClient client)
    {
      this.client = client;
    }

    public async Task<ProductDto> GetProduct(int id)
    {
      var response = await client.GetAsync($"/api/product/{id}");
      var apiContent = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          return null;
        }

        var error = JsonSerializer.Deserialize<ErrorDto>(apiContent);
        throw new ErrorException(response.StatusCode, error.ResponseCode, error.Message);
      }

      var resp = JsonSerializer.Deserialize<ProductDto>(apiContent);
      return resp;
    }

    public async Task<List<ProductDto>> GetProducts(List<int> ids)
    {
      var response = await client.GetAsync($"/api/product/?ids=" + String.Join("&ids=", ids));
      var apiContent = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        var error = JsonSerializer.Deserialize<ErrorDto>(apiContent);
        throw new ErrorException(response.StatusCode, error.ResponseCode, error.Message);
      }

      var resp = JsonSerializer.Deserialize<List<ProductDto>>(apiContent);
      return resp;
    }
  }
}
