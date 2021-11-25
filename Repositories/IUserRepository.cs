using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        IEnumerable<Listing> GetAllUserListingsByUserId(int userId);
        IEnumerable<Listing> GetAllRequestedListingsByUserId(int userId);
    }
}