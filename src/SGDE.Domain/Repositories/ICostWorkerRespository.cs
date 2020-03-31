namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;

    #endregion

    public interface ICostWorkerRepository
    {
        QueryResult<CostWorker> GetAll(int skip = 0, int take = 0, string filter = null, int userId = 0);
        CostWorker GetById(int id);
        CostWorker Add(CostWorker newCostWorker);
        bool Update(CostWorker costWorker);
        bool ValidateCostWorker(CostWorker costWorker);
        bool Delete(int id);
    }
}
