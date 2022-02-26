using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IAdvanceRepository
    {
        QueryResult<Advance> GetAll(int skip = 0, int take = 0, int userId = 0);
        Advance GetById(int id);
        Advance Add(Advance newAdvance);
        bool Update(Advance advance);
        bool Delete(int id);
    }
}
