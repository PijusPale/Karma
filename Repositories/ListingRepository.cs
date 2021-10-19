using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Karma.Models;

namespace Karma.Repositories
{
    public class ListingRepository : Repository<Listing>, IListingRepository
    {

        public ListingRepository(string filePath) : base(filePath) { }
        
    }
}