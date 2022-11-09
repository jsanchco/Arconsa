using SGDE.Domain.Entities;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IWorkCostRepository
    {
        List<WorkCost> GetAll(int workId);
        double SumAll(int workId);
        WorkCost GetById(int id);
        WorkCost Add(WorkCost newWorkCost);
        bool Update(WorkCost workCost);
        bool Delete(int id);
    }
}
