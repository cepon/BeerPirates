using BeerRecommendations.Database;

namespace BeerRecommendations.Repositories
{
  public interface IBeerRecommendationsRepository
  {
    Task<List<BeerRecommendation>> GetBeerRecommendationsAsync();
    Task<BeerRecommendation> GetBeerRecommendationByIdAsync(int id);
    Task AddBeerRecommendationAsync(BeerRecommendation beerRecommendation);
    Task UpdateBeerRecommendationAsync(BeerRecommendation beerRecommendation);
    Task DeleteBeerRecommendationAsync(int id);
    Task<BeerRecommendationProduct> GetBeerRecommendationProductAsync(int beerRecommendtaionId, int beerRecommendtaionProductId);
    Task AddBeerRecommendationProductAsync(BeerRecommendationProduct beerRecommendationProduct);
    Task DeleteBeerRecommendationProductsAsync(int beerRecommendationId, List<int> productIds);
  }
}
