namespace BeerRecommendations.DTOs
{
  public class BeerRecommendationDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductDto> Products { get; set; }
  }
}
