using Karma.Models;
using Microsoft.EntityFrameworkCore;

namespace Karma.data.messages
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<User> Users { get; set; }        
    }
}