using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Karma.Models;

namespace Karma.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected string _filePath;

        public Repository(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(TEntity entity)
        {
            List<TEntity> entities = GetAll().ToList();
            entities.Add(entity);
            writeEntitiesToFile(entities);
        }

        public IEnumerable<TEntity> GetAll()
        {
            string jsonString = System.IO.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TEntity>>(jsonString);
        }

        public TEntity GetById(string id)
        {
            List<TEntity> entities = GetAll().ToList();
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteById(string id)
        {
            List<TEntity> entities = GetAll().ToList();
            entities.Remove(entities.Find(x => x.Id == id));
            writeEntitiesToFile(entities);
        }

        public void Update(TEntity entity)
        {
            List<TEntity> entities = GetAll().ToList();
            entities[entities.FindIndex(l => l.Id == entity.Id)] = entity;
            writeEntitiesToFile(entities);
        }

        private void writeEntitiesToFile(List<TEntity> entities)
        {
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(entities, jsonOptions);
            System.IO.File.WriteAllText(_filePath, jsonString);
        }
    }
}