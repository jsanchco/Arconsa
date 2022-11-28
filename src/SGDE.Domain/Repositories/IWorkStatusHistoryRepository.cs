namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using System.Collections.Generic;

    #endregion

    public interface IWorkStatusHistoryRepository
    {
        List<WorkStatusHistory> GetAll(int workId = 0);
        WorkStatusHistory GetById(int id);
        WorkStatusHistory Add(WorkStatusHistory newWorkStatusHistory);
        bool Update(WorkStatusHistory workStatusHistory);
        bool Delete(int id);
    }
}
