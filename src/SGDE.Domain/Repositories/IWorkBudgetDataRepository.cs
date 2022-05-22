using SGDE.Domain.Entities;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IWorkBudgetDataRepository
    {
        List<WorkBudgetData> GetAll(int workId);
        WorkBudgetData GetById(int id);
        WorkBudgetData Add(WorkBudgetData newWorkBudgetData);
        bool Update(WorkBudgetData workBudgetData);
        bool Delete(int id);
    }
}
