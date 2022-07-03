using Product.Database.Inherit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Database
{
  public class Tag : DefaultFields
  {
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductTags> ProductTags { get; set; }
  }
}
