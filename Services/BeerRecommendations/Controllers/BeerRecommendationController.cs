using BeerRecommendations.DTOs;
using BeerRecommendations.Services.BeerRecommendationsService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeerRecommendations.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BeerRecommendationController : ControllerBase
  {
    private readonly IBeerRecommendationsService _beerRecommendationsService;

    public BeerRecommendationController(IBeerRecommendationsService beerRecommendationsService)
    {
      _beerRecommendationsService = beerRecommendationsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BeerRecommendationDto>>> GetBeerRecommendationAsync()
    {
      return Ok(await _beerRecommendationsService.GetBeerRecommendationsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BeerRecommendationDto>> GetBeerRecommendationAsync(int id)
    {
      return Ok(await _beerRecommendationsService.GetBeerRecommendationByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> AddBeerRecommendationAsync(BeerRecommendationDto beerRecommendation)
    {
      return Ok(await _beerRecommendationsService.AddBeerRecommendationAsync(beerRecommendation));
    }

    [HttpPut]
    public async Task<ActionResult<bool>> UpdateBeerRecommendationAsync(BeerRecommendationDto beerRecommendation)
    {
      return Ok(await _beerRecommendationsService.UpdateBeerRecommendationAsync(beerRecommendation));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteBeerRecommendationAsync(int id)
    {
      return Ok(await _beerRecommendationsService.DeleteBeerRecommendationAsync(id));
    }
  }
}
