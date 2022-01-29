using SGDE.Domain.Entities;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IWorkBudgetRepository
    {
        List<WorkBudget> GetAllLite(int workId = 0);
        List<WorkBudget> GetAll(int workId);
        WorkBudget GetById(int id);
        WorkBudget Add(WorkBudget newWorkBudget);
        bool Update(WorkBudget workBudget);
        bool Delete(int id);
    }
}
