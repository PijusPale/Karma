using System.Collections.Generic;
using System.Threading.Tasks;

namespace Karma.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(string id);

        void Add(TEntity entity);

        void DeleteById(string id);

        void Update(TEntity id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(string id);

        Task AddAsync(TEntity entity);

        Task DeleteByIdAsync(string id);

        Task UpdateAsync(TEntity entity);
    }
}