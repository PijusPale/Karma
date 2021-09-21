using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IListingRepository 
    {
        IEnumerable<Listing> GetAllListings();

        Listing GetListingById(string id);

        void AddListing(Listing listing);        
    }
}