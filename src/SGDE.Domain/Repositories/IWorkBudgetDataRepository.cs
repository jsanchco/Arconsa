using SGDE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SGDE.Domain.Repositories
{
    public interface IWorkBudgetDataRepository
    {
        List<WorkBudgetData> GetAll(int workId  = 0);
        WorkBudgetData GetById(int id);
        WorkBudgetData GetByWorkIdAndReference(int workId, string reference);
        WorkBudgetData Add(WorkBudgetData newWorkBudgetData);
        bool Update(WorkBudgetData workBudgetData);
        bool Delete(int id);
    }
}
