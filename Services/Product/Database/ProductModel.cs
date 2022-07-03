using Product.Database.Inherit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Database
{
  public class ProductModel : DefaultFields
  {
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    [MaxLength(200)]
    [Required]
    public string Name { get; set; }
    public byte[] Image { get; set; }
    [Column(TypeName = "numeric(19, 4)")]
    public decimal Price { get; set; }
    public string Details { get; set; }
    public DateTime? ListedSince { get; set; }
    public int SoldQty { get; set; }
    public int Stock { get; set; }
    public int BrandId { get; set; }
    [ForeignKey("BrandId")]
    public Brand Brand { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    public List<ProductTags> ProductTags { get; set; }
  }
}
