using Product.Database.Inherit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Database
{
  public class ProductTags : DefaultFields
  {
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public ProductModel Product { get; set; }
    public int TagId { get; set; }
    [ForeignKey("TagId")]
    public Tag Tag { get; set; }
  }
}
