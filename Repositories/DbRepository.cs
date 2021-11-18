using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.data.messages;
using Karma.Models;
using Microsoft.EntityFrameworkCore;

namespace Karma.Repositories
{
    public class DbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public readonly BaseDbContext _context;

        public DbSet<TEntity> entities { get; set; }

        public DbRepository(BaseDbContext context)
        {
            _context = context;
            entities = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entities.Add(entity);
            _context.SaveChanges();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            try
            {
                await entities.AddAsync(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteById(int id)
        {
            entities.Remove(entities.Find(id));
            _context.SaveChanges();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                entities.Remove(await entities.FindAsync(id));
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            return entities;
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<TEntity>)entities);
        }

        public TEntity GetById(int id)
        {
            return entities.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await entities.FindAsync(id);
        }

        public void Update(TEntity id)
        {
            entities.Remove(GetById(id.Id));
            entities.Add(id);
            _context.SaveChanges();
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                entities.Remove(await GetByIdAsync(entity.Id));
                await entities.AddAsync(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}