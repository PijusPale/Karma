using System.ComponentModel.DataAnnotations;

namespace Karma.Models
{
    public abstract class Entity
    {
        [Key]
        public string Id { get; set; }
        public Entity()
        {

        }
        public Entity(string id)
        {
            Id = id;
        }
    }
}