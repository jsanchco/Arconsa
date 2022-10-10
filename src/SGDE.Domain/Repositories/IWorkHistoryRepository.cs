using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface IWorkHistoryRepository
    {
        QueryResult<WorkHistory> GetAll(int workId = 0, int skip = 0, int take = 0, string filter = null);
        WorkHistory GetById(int id);
        WorkHistory Add(WorkHistory newWorkHistory);
        bool Update(WorkHistory workHistory);
        bool Delete(int id);
    }
}
