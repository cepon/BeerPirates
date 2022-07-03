using Microsoft.EntityFrameworkCore;

namespace Product.Database.Context
{
  public class DataContext : DbContext
  { 
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<ProductModel> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProductTags> ProductTags { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
  }
}
