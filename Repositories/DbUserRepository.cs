using System.Collections.Generic;
using Karma.data.messages;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbUserRepository: DbRepository<User>, IUserRepository
    {
        public DbUserRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Users;
        }
    }
}