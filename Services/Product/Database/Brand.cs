using Product.Database.Inherit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Database
{
  public class Brand : DefaultFields
  {
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
