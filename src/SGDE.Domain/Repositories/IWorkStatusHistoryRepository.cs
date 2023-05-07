namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;

    #endregion

    public interface IWorkStatusHistoryRepository
    {
        List<WorkStatusHistory> GetAll(int enterpriseId = 0, int workId = 0);
        List<WorkStatusHistory> GetAllBetweenDates(DateTime startDate, DateTime endDate);
        WorkStatusHistory GetById(int id);
        WorkStatusHistory Add(WorkStatusHistory newWorkStatusHistory);
        bool Update(WorkStatusHistory workStatusHistory);
        bool Delete(int id);
    }
}
