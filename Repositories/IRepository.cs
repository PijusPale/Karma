using System.Collections.Generic;
using System.Threading.Tasks;

namespace Karma.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        void Add(TEntity entity);

        void DeleteById(int id);

        void Update(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task<bool> AddAsync(TEntity entity);

        Task<bool> DeleteByIdAsync(int id);

        Task<bool> UpdateAsync(TEntity entity);
    }
}