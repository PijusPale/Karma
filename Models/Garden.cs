using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Karma.Models
{
    public class Garden: Entity
    {
        // public Garden(int userId, int size)
        // {
        //     UserId = userId;
        //     Plants = new string[size, size];
        // }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }

        public List<List<string>> Plants;
    }
}