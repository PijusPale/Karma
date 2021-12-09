using System.Collections.Generic;
using Karma.Models;
using Karma.Database;
using Microsoft.Extensions.Logging;

namespace Karma.Repositories
{
    public class CategoryRepository : DbRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(BaseDbContext context) : base(context)
        {
        }
    }
}