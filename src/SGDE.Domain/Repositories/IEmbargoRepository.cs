using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface IEmbargoRepository
    {
        QueryResult<Embargo> GetAll(int skip = 0, int take = 0, int userId = 0);
        Embargo GetById(int id);
        Embargo Add(Embargo newEmbargo);
        bool Update(Embargo embargo);
        bool Delete(int id);
    }
}
