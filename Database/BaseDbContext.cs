using Karma.Models;
using Microsoft.EntityFrameworkCore;

namespace Karma.Database
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany<Listing>(u => u.RequestedListings)
                .WithMany(l => l.Requestees);
            modelBuilder
                .Entity<User>()
                .HasMany<Listing>(u => u.Listings)
                .WithOne(l => l.User);
        }
    }
}