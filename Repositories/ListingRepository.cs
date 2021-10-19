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
        public void Add(Listing listing)
        {
            List<Listing> listings = GetAll().ToList();
            listings.Add(listing);
            writeListingsToFile(listings);
        }

        public IEnumerable<Listing> GetAll()
        {
            string jsonString = System.IO.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Listing>>(jsonString);
        }

        public Listing GetById(string id)
        {
            List<Listing> listings = GetAll().ToList();
            return listings.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteById(string id)
        {
            List<Listing> listings = GetAll().ToList();
            listings.Remove(listings.Find(x => x.Id == id));
            writeListingsToFile(listings);
        }

        public void Update(Listing listing)
        {
            List<Listing> listings = GetAll().ToList();
            listings[listings.FindIndex(l => l.Id == listing.Id)] = listing;
            writeListingsToFile(listings);
        }

        private void writeListingsToFile(List<Listing> listings)
        {
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(listings, jsonOptions);
            System.IO.File.WriteAllText(_filePath, jsonString);
        }
    }
}