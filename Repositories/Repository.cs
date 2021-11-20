using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.Extensions.Logging;

namespace Karma.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private static object locker = new object();
        protected string _filePath;
        private readonly int numberOfRetries = 3;
        private readonly int DelayOnRetry = 1000;
        private readonly ILogger<Repository<TEntity>> _logger;

        public Repository(string filePath, ILogger<Repository<TEntity>> logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        public void Add(TEntity entity)
        {
            var random = new Random();
            entity.Id = random.Next(9999); // temp fix for id generation, later this should be assigned in DB.
            List<TEntity> entities = GetAll().ToList();
            entities.Add(entity);
            IEnumerable<TEntity> queryAscending = from ent in entities
                                                  orderby ent.Id
                                                  select ent;

            writeEntitiesToFile(queryAscending.ToList());
        }

        public IEnumerable<TEntity> GetAll()
        {
            lock(locker) {
                try
                {
                    string jsonString = System.IO.File.ReadAllText(_filePath);
                    return JsonSerializer.Deserialize<List<TEntity>>(jsonString);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Repository read file exception");
                    return new List<TEntity>();
                }
            }

        }

        public TEntity GetById(int id)
        {
            List<TEntity> entities = GetAll().ToList();
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteById(int id)
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
            lock (locker) {
                try
                {
                    System.IO.File.WriteAllText(_filePath, jsonString);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Repository write to file exception");
                }
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                string jsonString = await System.IO.File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<TEntity>>(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository read file async exception");
                return null;
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            List<TEntity> entities = (await GetAllAsync()).ToList();
            if (entities == null)
                return null;
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            var random = new Random();
            entity.Id = random.Next(9999); // temp fix for id generation, later this should be assigned in DB.
            List<TEntity> entities = (await GetAllAsync()).ToList();
            if (entities == null)
                return false;
            entities.Add(entity);

            return await writeEntitiesToFileAsync(entities);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            List<TEntity> entities = (await GetAllAsync()).ToList();
            if (entities == null)
                return false;
            entities.Remove(entities.Find(x => x.Id == id));
            return await writeEntitiesToFileAsync(entities);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            List<TEntity> entities = (await GetAllAsync()).ToList();
            if (entities == null)
                return false;
            entities[entities.FindIndex(l => l.Id == entity.Id)] = entity;
            return await writeEntitiesToFileAsync(entities);
        }

        private async Task<bool> writeEntitiesToFileAsync(List<TEntity> entities)
        {
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(entities, jsonOptions);
            for (int i = 0; i <= numberOfRetries; ++i)
            {
                try
                {
                    await System.IO.File.WriteAllTextAsync(_filePath, jsonString);
                    return true;
                }
                catch (IOException) when (i < numberOfRetries)
                {
                    await Task.Delay(DelayOnRetry);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Repository write to file async exception");
                    return false;
                }
            }
            return false;
        }
    }
}