using BeerRecommendations.Database.Inherit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommendations.Database
{
  public class BeerRecommendationProduct : DefaultFields
  {
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    public int BeerRecommendationId { get; set; }
    [ForeignKey("BeerRecommendationId")]
    public BeerRecommendation BeerRecommendation { get; set; }
    public int ProductId { get; set; }
  }
}
