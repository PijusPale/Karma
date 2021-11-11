using System.Collections.Generic;
using Karma.Models;
using Microsoft.Extensions.Logging;

namespace Karma.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(string filePath, ILogger<CategoryRepository> logger) : base(filePath, logger)
        {
        }
    }
}