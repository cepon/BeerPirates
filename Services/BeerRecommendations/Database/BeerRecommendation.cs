using BeerRecommendations.Database.Inherit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommendations.Database
{
  public class BeerRecommendation : DefaultFields
  {
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    [MaxLength(200)]
    [Required]
    public string Name { get; set; }
    public List<BeerRecommendationProduct> Products { get; set; }
  }
}
