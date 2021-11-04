using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(string filePath) : base(filePath)
        {
        }
    }
}