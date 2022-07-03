using Microsoft.EntityFrameworkCore;

namespace BeerRecommendations.Database.Context
{
  public class DataContext : DbContext
  { 
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<BeerRecommendation> BeerRecommendations { get; set; }
    public DbSet<BeerRecommendationProduct> BeerRecommendationProducts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
  }
}
