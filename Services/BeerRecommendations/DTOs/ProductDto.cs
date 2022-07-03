using System.ComponentModel.DataAnnotations;

namespace BeerRecommendations.DTOs
{
  public class ProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public decimal Price { get; set; }
    public string Details { get; set; }
    public DateTime? ListedSince { get; set; }
    public int SoldQty { get; set; }
    public int Stock { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
    public List<string> Tags { get; set; }
  }
}
