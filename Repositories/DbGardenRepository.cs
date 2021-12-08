using System.Linq;
using System.Threading.Tasks;
using Karma.Database;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbGardenRepository : DbRepository<Garden>, IGardenRepository
    {
        public DbGardenRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Gardens;
        }

        public Garden GetByUserId(int userId)
        {
            return entities.FirstOrDefault(g => g.UserId == userId);
        }
    }
}