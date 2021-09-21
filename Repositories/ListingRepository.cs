using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Karma.Models;

namespace Karma.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly string _filePath;

        public ListingRepository(string filePath)
        {
            _filePath = filePath;
        }
        public void AddListing(Listing listing)
        {
            List<Listing> listings = GetAllListings().ToList();
            listings.Add(listing);
            string newJsonString = JsonSerializer.Serialize(listings);
            System.IO.File.WriteAllText(_filePath, newJsonString);
        }

        public IEnumerable<Listing> GetAllListings()
        {
            string jsonString = System.IO.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Listing>>(jsonString);
        }

        public Listing GetListingById(string id)
        {            
            List<Listing> listings = GetAllListings().ToList();
            return listings.FirstOrDefault(x => x.Id == id);
        }
    }
}