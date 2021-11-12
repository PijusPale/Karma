using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IListingRepository : IRepository<Listing>
    {
        Task<IEnumerable<Listing>> GetAllUserListingsAsync(string userId);
        Task<IEnumerable<Listing>> GetRequestedListingsAsync(string userId);
    }
}