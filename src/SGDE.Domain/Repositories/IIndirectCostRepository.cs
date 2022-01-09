using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface IIndirectCostRepository
    {
        QueryResult<IndirectCost> GetAll(int skip = 0, int take = 0);
        IndirectCost GetById(int id);
        IndirectCost Add(IndirectCost newIndirectCost);
        bool Update(IndirectCost indirectCost);
        bool Delete(int id);
    }
}
