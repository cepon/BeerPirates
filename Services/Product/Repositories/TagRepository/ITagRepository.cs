using Product.Database;

namespace Product.Repositories.TagRepository
{
  public interface ITagRepository
  {
    Task<Tag> GetTagByNameAsync(string name);
    Task AddTagAsync(Tag tag);
  }
}
