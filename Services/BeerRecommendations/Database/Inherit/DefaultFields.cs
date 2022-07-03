
using BeerRecommendations.Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommendations.Database.Inherit
{
  public class DefaultFields
  {
    [Column(Order = 1)]
    [Required]
    public DateTime DateCreate { get; set; }
    [Column(Order = 2)]
    [Required]
    public DateTime DateUpdate { get; set; }
    [Column(Order = 3)]
    [Required]
    public ActiveEnum Active { get; set; }

    public DefaultFields()
    {
      DateCreate = DateTime.UtcNow;
      DateUpdate = DateTime.UtcNow;
      Active = ActiveEnum.Active;
    }
  }
}
