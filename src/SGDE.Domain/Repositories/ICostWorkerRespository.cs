namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using SGDE.Domain.ViewModels;
    using System.Collections.Generic;
    using System;

    #endregion

    public interface ICostWorkerRepository
    {
        QueryResult<CostWorker> GetAll(int skip = 0, int take = 0, string filter = null, int userId = 0);
        List<CostWorker> GetCostWorkerBetweenDates(DateTime startDate, DateTime endDate);
        CostWorker GetById(int id);
        CostWorker Add(CostWorker newCostWorker);
        bool Update(CostWorker costWorker);
        bool ValidateCostWorker(CostWorker costWorker);
        bool Delete(int id);
    }
}
