using Product.Database;
using Product.DTOs;
using Product.Exceptions;
using Product.Repositories.BrandRepository;
using Product.Repositories.CategoryRepository;
using Product.Repositories.ProductRepository;
using Product.Repositories.TagRepository;

namespace Product.Services.ProductService
{
  public class ProductService : IProductService
  {
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;

    public ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
    {
      _logger = logger;
      _productRepository = productRepository;
      _brandRepository = brandRepository;
      _categoryRepository = categoryRepository;
      _tagRepository = tagRepository;
    }

    public async Task<bool> AddProductAsync(ProductDto product)
    {
      await createOrUpdateProductModel(product, false);
      return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
      await _productRepository.DeleteProductAsync(id);
      return true;
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
      ProductModel product = await _productRepository.GetProductByIdAsync(id);

      if (product == null)
      {
        _logger.LogError($"Product with id={id} does not exists.");
        throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, "Product does not exist");
      }

      return new ProductDto()
      {
        Id = product.Id,
        Name = product.Name,
        Details = product.Details,
        Image = product.Image,
        ListedSince = product.ListedSince,
        SoldQty = product.SoldQty,
        Stock = product.Stock,
        Price = product.Price,
        Brand = product.Brand.Name,
        Category = product.Category.Name,
        Tags = product.ProductTags.Select(x => x.Tag.Name).ToList(),
      };
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
      List<ProductModel> products = await _productRepository.GetProductsAsync();

      return products
        .Select(p => new ProductDto()
        {
          Id = p.Id,
          Name = p.Name,
          Details = p.Details,
          Image = p.Image,
          ListedSince = p.ListedSince,
          SoldQty = p.SoldQty,
          Stock = p.Stock,
          Price = p.Price,
          Brand = p.Brand.Name,
          Category = p.Category.Name,
          Tags = p.ProductTags.Select(x => x.Tag.Name).ToList(),
        })
        .ToList();
    }

    public async Task<List<ProductDto>> GetProductsByIdsAsync(List<int> ids)
    {
      List<ProductModel> products = await _productRepository.GetProductsByIdsAsync(ids);

      return products
        .Select(p => new ProductDto()
        {
          Id = p.Id,
          Name = p.Name,
          Details = p.Details,
          Image = p.Image,
          ListedSince = p.ListedSince,
          SoldQty = p.SoldQty,
          Stock = p.Stock,
          Price = p.Price,
          Brand = p.Brand.Name,
          Category = p.Category.Name,
          Tags = p.ProductTags.Select(x => x.Tag.Name).ToList(),
        })
        .ToList();
    }

    public async Task<bool> UpdateProductAsync(ProductDto product)
    {
      await createOrUpdateProductModel(product, true);
      return true;
    }

    private async Task createOrUpdateProductModel(ProductDto product, bool update)
    {
      ProductModel productModel = new ProductModel();
      if (update)
      {
        productModel = await _productRepository.GetProductByIdAsync(product.Id);

        if (productModel == null)
        {
          _logger.LogError($"Product with id={product.Id} does not exists.");
          throw new ErrorException(System.Net.HttpStatusCode.NotFound, Enums.ErrorEnum.DOES_NOT_EXIST, "Product does not exist");
        }
      }

      productModel.Name = product.Name;
      productModel.ListedSince = product.ListedSince;
      productModel.SoldQty = product.SoldQty;
      productModel.Stock = product.Stock;
      productModel.Price = product.Price;
      productModel.Details  = product.Details;
      productModel.Image = product.Image;

      Category category = await _categoryRepository.GetCategoryByNameAsync(product.Category);
      if (category == null)
      {
        category = new Category();
        category.Name = product.Category;
        await _categoryRepository.AddCategoryAsync(category);
      }

      productModel.CategoryId = category.Id;

      Brand brand = await _brandRepository.GetBrandByNameAsync(product.Brand);
      if (brand == null)
      {
        brand = new Brand();
        brand.Name = product.Brand;
        await _brandRepository.AddBrandAsync(brand);
      }

      productModel.BrandId = brand.Id;

      List<int> tagIds = new List<int>();
      foreach (string tag in product.Tags)
      {
        Tag tagModel = await _tagRepository.GetTagByNameAsync(tag);
        if (tagModel == null)
        {
          tagModel = new Tag();
          tagModel.Name = tag;
          await _tagRepository.AddTagAsync(tagModel);
        }

        tagIds.Add(tagModel.Id);
      }

      if (update)
      {
        await _productRepository.UpdateProductAsync(productModel);
      }
      else
      {
        await _productRepository.AddProductAsync(productModel);
      }

      foreach (int tagId in tagIds)
      {
        ProductTags productTags = await _productRepository.GetProductTagAsync(productModel.Id, tagId);
        if (productTags == null)
        {
          productTags = new ProductTags();
          productTags.ProductId = productModel.Id;
          productTags.TagId = tagId;
          await _productRepository.AddProductTagAsync(productTags);
          _logger.LogInformation($"Added tag with id {productTags.TagId} to product with id {productTags.ProductId}");
        }
      }

      await _productRepository.DeleteProductTagsAsync(productModel.Id, tagIds);
      _logger.LogInformation($"Removed unused tags from product");
    }
  }
}
