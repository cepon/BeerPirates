using BeerRecommendations.Database;
using BeerRecommendations.Database.Context;
using BeerRecommendations.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BeerRecommendations.Repositories
{
  public class BeerRecommendationsRepository : IBeerRecommendationsRepository
  {

    private readonly DataContext _context;
    private readonly ILogger<BeerRecommendationsRepository> _logger;

    public BeerRecommendationsRepository(DataContext context, ILogger<BeerRecommendationsRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task AddBeerRecommendationAsync(BeerRecommendation beerRecommendation)
    {
      _context.BeerRecommendations.Add(beerRecommendation);
      await _context.SaveChangesAsync();
    }

    public async Task AddBeerRecommendationProductAsync(BeerRecommendationProduct beerRecommendationProduct)
    {
      _context.BeerRecommendationProducts.Add(beerRecommendationProduct);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteBeerRecommendationAsync(int id)
    {
      BeerRecommendation beerRecommendation = await _context.BeerRecommendations
        .Where(br => br.Id == id && br.Active == Database.Enums.ActiveEnum.Active)
        .FirstOrDefaultAsync();

      if (beerRecommendation == null)
      {
        _logger.LogError($"Beer recommendation with id={id} does not exists.");
        throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, "Beer recommendation does not exist");
      }

      beerRecommendation.Active = Database.Enums.ActiveEnum.Deleted;
      beerRecommendation.DateUpdate = DateTime.UtcNow;
      await _context.SaveChangesAsync();
      _logger.LogInformation($"Beer recommendation with id={id} is deleted.");
    }

    public async Task DeleteBeerRecommendationProductsAsync(int beerRecommendationId, List<int> productIds)
    {
      List<BeerRecommendationProduct> beerRecommendationProductsToDelete = await _context.BeerRecommendationProducts
         .Where(brp => brp.BeerRecommendationId == beerRecommendationId && !productIds.Contains(brp.ProductId) && brp.Active == Database.Enums.ActiveEnum.Active)
         .ToListAsync();

      beerRecommendationProductsToDelete.ForEach(pt =>
      {
        pt.DateUpdate = DateTime.UtcNow;
        pt.Active = Database.Enums.ActiveEnum.Deleted;
      });
      await _context.SaveChangesAsync();
    }

    public async Task<BeerRecommendation> GetBeerRecommendationByIdAsync(int id)
    {
      BeerRecommendation beerRecommendation = await _context.BeerRecommendations
        .Where(br => br.Id == id && br.Active == Database.Enums.ActiveEnum.Active)
        .Include(br => br.Products.Where(p => p.Active == Database.Enums.ActiveEnum.Active))
        .FirstOrDefaultAsync();

      return beerRecommendation;
    }

    public async Task<BeerRecommendationProduct> GetBeerRecommendationProductAsync(int beerRecommendtaionId, int productId)
    {
      BeerRecommendationProduct beerRecommendationProduct = await _context.BeerRecommendationProducts
        .Where(brp => brp.BeerRecommendationId == beerRecommendtaionId && brp.ProductId == productId && brp.Active == Database.Enums.ActiveEnum.Active)
        .FirstOrDefaultAsync();

      return beerRecommendationProduct;
    }

    public async Task<List<BeerRecommendation>> GetBeerRecommendationsAsync()
    {
      List<BeerRecommendation> beerRecommendations = await _context.BeerRecommendations
        .Where(br => br.Active == Database.Enums.ActiveEnum.Active)
        .Include(br => br.Products.Where(p => p.Active == Database.Enums.ActiveEnum.Active))
        .ToListAsync();

      return beerRecommendations;
    }

    public async Task UpdateBeerRecommendationAsync(BeerRecommendation beerRecommendation)
    {
      BeerRecommendation beerRecommendationModel = await GetBeerRecommendationByIdAsync(beerRecommendation.Id);
      beerRecommendation.DateUpdate = DateTime.UtcNow;
      _context.Entry(beerRecommendationModel).CurrentValues.SetValues(beerRecommendation);
      await _context.SaveChangesAsync();
    }
  }
}
