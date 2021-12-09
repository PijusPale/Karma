using Karma.Models;
using Karma.Database;

namespace Karma.Repositories
{
    public class CategoryRepository : DbRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(BaseDbContext context) : base(context)
        {
        }
    }
}