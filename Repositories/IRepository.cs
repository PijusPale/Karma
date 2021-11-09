using System.Collections.Generic;

namespace Karma.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(string id);

        void Add(TEntity entity);

        void DeleteById(string id);

        void Update(TEntity id);
    }
}