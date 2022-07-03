using BeerRecommendations.Database;
using BeerRecommendations.Database.Context;
using BeerRecommendations.DTOs;
using BeerRecommendations.Exceptions;
using BeerRecommendations.Repositories;
using BeerRecommendations.Services.Consumed.ProductService;

namespace BeerRecommendations.Services.BeerRecommendationsService
{
  public class BeerRecommendationsService : IBeerRecommendationsService
  {

    private readonly ILogger<BeerRecommendationsService> _logger;
    private readonly IBeerRecommendationsRepository _beerRecommendationsRepository;
    private readonly IProductService _productService;

    public BeerRecommendationsService(ILogger<BeerRecommendationsService> logger, IBeerRecommendationsRepository beerRecommendationsRepository, IProductService productService)
    {
      _logger = logger;
      _beerRecommendationsRepository = beerRecommendationsRepository;
      _productService = productService;
    }

    public async Task<bool> AddBeerRecommendationAsync(BeerRecommendationDto beerRecommendationDto)
    {
      await createOrUpdateBeerRecommendationModel(beerRecommendationDto, false);
      return true;
    }

    public async Task<bool> DeleteBeerRecommendationAsync(int id)
    {
      await _beerRecommendationsRepository.DeleteBeerRecommendationAsync(id);
      return true;
    }

    public async Task<BeerRecommendationDto> GetBeerRecommendationByIdAsync(int id)
    {
      BeerRecommendation beerRecommendation = await _beerRecommendationsRepository.GetBeerRecommendationByIdAsync(id);

      if (beerRecommendation == null)
      {
        _logger.LogError($"Beer recommendation with id={id} does not exists.");
        throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, "Beer recommendation does not exist");
      }

      List<ProductDto> products;
      products = await _productService.GetProducts(beerRecommendation.Products.Select(p => p.ProductId).ToList());

      return new BeerRecommendationDto()
      {
        Id = beerRecommendation.Id,
        Name = beerRecommendation.Name,
        Products = products
      };
    }

    public async Task<List<BeerRecommendationDto>> GetBeerRecommendationsAsync()
    {
      List<BeerRecommendation> beerRecommendations = await _beerRecommendationsRepository.GetBeerRecommendationsAsync();

      List<BeerRecommendationDto> beerRecommendationsList = new List<BeerRecommendationDto>();
      foreach(BeerRecommendation br in beerRecommendations)
      {
        BeerRecommendationDto beerRecommendationDto = new BeerRecommendationDto();
        beerRecommendationDto.Id = br.Id;
        beerRecommendationDto.Name = br.Name;
        beerRecommendationDto.Products = await _productService.GetProducts(br.Products.Select(p => p.ProductId).ToList());
        beerRecommendationsList.Add(beerRecommendationDto);
      }

      return beerRecommendationsList;
    }

    public async Task<bool> UpdateBeerRecommendationAsync(BeerRecommendationDto beerRecommendationDto)
    {
      await createOrUpdateBeerRecommendationModel(beerRecommendationDto, true);
      return true;
    }

    private async Task createOrUpdateBeerRecommendationModel(BeerRecommendationDto beerRecommendation, bool update)
    {
      BeerRecommendation beerRecommendationModel = new BeerRecommendation();
      if (update)
      {
        beerRecommendationModel = await _beerRecommendationsRepository.GetBeerRecommendationByIdAsync(beerRecommendation.Id);

        if (beerRecommendationModel == null)
        {
          _logger.LogError($"Beer recommendation with id={beerRecommendation.Id} does not exists.");
          throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, "Beer recommendation does not exist");
        }
      }

      beerRecommendationModel.Name = beerRecommendation.Name;

      List<int> productIds = new List<int>();
      foreach(ProductDto product in beerRecommendation.Products)
      {
        ProductDto seekingProduct = await _productService.GetProduct(product.Id);
        if(seekingProduct == null)
        {
          _logger.LogError($"Product with id={product.Id} does not exists.");
          throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, $"Product with id={product.Id} does not exists.");
        }

        productIds.Add(seekingProduct.Id);
      }

      if (update)
      {
        await _beerRecommendationsRepository.UpdateBeerRecommendationAsync(beerRecommendationModel);
      }
      else
      {
        await _beerRecommendationsRepository.AddBeerRecommendationAsync(beerRecommendationModel);
      }

      foreach (int productId in productIds)
      {
        BeerRecommendationProduct beerRecommendationProduct = await _beerRecommendationsRepository.GetBeerRecommendationProductAsync(beerRecommendationModel.Id, productId);
        if (beerRecommendationProduct == null)
        {
          beerRecommendationProduct = new BeerRecommendationProduct();
          beerRecommendationProduct.BeerRecommendationId = beerRecommendationModel.Id;
          beerRecommendationProduct.ProductId = productId;
          await _beerRecommendationsRepository.AddBeerRecommendationProductAsync(beerRecommendationProduct);
          _logger.LogInformation($"Added product with id {beerRecommendationProduct.ProductId} to beer recommendation with id {beerRecommendationProduct.BeerRecommendationId}");
        }
      }

      await _beerRecommendationsRepository.DeleteBeerRecommendationProductsAsync(beerRecommendationModel.Id, productIds);
      _logger.LogInformation($"Removed unused tags from product");
    }
  }
}
