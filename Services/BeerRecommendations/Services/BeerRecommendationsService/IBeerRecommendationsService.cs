using BeerRecommendations.DTOs;

namespace BeerRecommendations.Services.BeerRecommendationsService
{
  public interface IBeerRecommendationsService
  {
    Task<List<BeerRecommendationDto>> GetBeerRecommendationsAsync();
    Task<BeerRecommendationDto> GetBeerRecommendationByIdAsync(int id);
    Task<bool> AddBeerRecommendationAsync(BeerRecommendationDto beerRecommendationDto);
    Task<bool> UpdateBeerRecommendationAsync(BeerRecommendationDto beerRecommendationDto);
    Task<bool> DeleteBeerRecommendationAsync(int id);
  }
}
