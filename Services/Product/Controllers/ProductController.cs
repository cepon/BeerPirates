using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.DTOs;
using Product.Services.ProductService;

namespace Product.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
      _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProductsAsync([FromQuery] int[] ids)
    {
      if(ids.Length == 0)
      {
        return Ok(await _productService.GetProductsAsync());
      }
      else
      {
        return Ok(await _productService.GetProductsByIdsAsync(ids.ToList()));
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
    {
      return Ok(await _productService.GetProductByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> AddProductAsync(ProductDto product)
    {
      return Ok(await _productService.AddProductAsync(product));
    }

    [HttpPut]
    public async Task<ActionResult<bool>> UpdateProductAsync(ProductDto product)
    {
      return Ok(await _productService.UpdateProductAsync(product));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteProductAsync(int id)
    {
      return Ok(await _productService.DeleteProductAsync(id));
    }
  }
}
