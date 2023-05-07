namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using SGDE.Domain.ViewModels;
    using System;
    using System.Collections.Generic;

    #endregion

    public interface IWorkRepository
    {
        List<Work> GetAllLite(string filter = null, int clientId = 0);
        List<Work> GetAllLiteIncludeClient(int enterpriseId = 0, string filter = null);
        QueryResult<Work> GetAll(int skip = 0, int take = 0, int enterpriseId = 0, string filter = null, int clientId = 0, bool showCloseWorks = true);
        List<Work> GetAllWorkBetweenDates(int enterpriseId, DateTime startDate, DateTime endDate);
        Work GetById(int id);
        Work Add(Work newWork);
        bool Update(Work work);
        bool Delete(int id);
        int Count();
    }
}
