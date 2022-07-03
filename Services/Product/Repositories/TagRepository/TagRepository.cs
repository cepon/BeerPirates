using Microsoft.EntityFrameworkCore;
using Product.Database;
using Product.Database.Context;

namespace Product.Repositories.TagRepository
{
  public class TagRepository : ITagRepository
  {
    private readonly DataContext _context;
    private readonly ILogger<TagRepository> _logger;

    public TagRepository(DataContext context, ILogger<TagRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task AddTagAsync(Tag tag)
    {
      _context.Tags.Add(tag);
      await _context.SaveChangesAsync();
      _logger.LogInformation($"Created tag {tag.Name}");
    }

    public async Task<Tag> GetTagByNameAsync(string name)
    {
      Tag tagModel = await _context.Tags
           .Where(t => t.Name == name && t.Active == Database.Enums.ActiveEnum.Active)
           .FirstOrDefaultAsync();

      return tagModel;
    }
  }
}
