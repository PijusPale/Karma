using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IGardenRepository: IRepository<Garden>
    {
        Garden GetByUserId(int userId);         
    }
}