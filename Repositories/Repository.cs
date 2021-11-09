using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
            var random = new Random();
            entity.Id = random.Next(9999).ToString(); // temp fix for id generation, later this should be assigned in DB.
            List<TEntity> entities = GetAll().ToList();
            entities.Add(entity);
            IEnumerable<TEntity> queryAscending = from ent in entities
                                 orderby ent.Id
                                 select ent;

            writeEntitiesToFile(queryAscending.ToList());
        }

        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                string jsonString = System.IO.File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TEntity>>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }

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
            try
            {
                System.IO.File.WriteAllText(_filePath, jsonString);
            }
            catch
            {
                // TODO: Add logging
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                string jsonString = await System.IO.File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<TEntity>>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            List<TEntity> entities = (await GetAllAsync()).ToList();
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            var random = new Random();
            entity.Id = random.Next(9999).ToString(); // temp fix for id generation, later this should be assigned in DB.
            List<TEntity> entities = (await GetAllAsync()).ToList();
            entities.Add(entity);

            await writeEntitiesToFileAsync(entities);
        }

        public async Task DeleteByIdAsync(string id)
        {
            List<TEntity> entities = (await GetAllAsync()).ToList();
            entities.Remove(entities.Find(x => x.Id == id));
            await writeEntitiesToFileAsync(entities);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            List<TEntity> entities = (await GetAllAsync()).ToList();
            entities[entities.FindIndex(l => l.Id == entity.Id)] = entity;
            await writeEntitiesToFileAsync(entities);
        }

        private async Task writeEntitiesToFileAsync(List<TEntity> entities)
        {
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(entities, jsonOptions);
            try
            {
                await System.IO.File.WriteAllTextAsync(_filePath, jsonString);
            }
            catch
            {
                // TODO: Add logging
            }
        }
    }
}