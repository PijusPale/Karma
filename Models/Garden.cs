using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Karma.Models
{
    public class Garden: Entity
    {
        public Garden() {
            Plants = new List<List<string>>(); 
            for (int i = 0; i < 9; i++) 
                Plants.Add(Enumerable.Repeat(string.Empty, 9).ToList());
        }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public List<List<string>> Plants;
    }
}