namespace SGDE.Domain.Repositories
{
    #region Using

    using Entities;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;

    #endregion

    public interface IDailySigningRepository
    {
        QueryResult<DailySigning> GetAll(int skip = 0, int take = 0, int userId = 0);
        List<DailySigning> GetHistoryByUserId(int userId);
        List<DailySigning> GetHistoryByWorkId(int workId);
        List<DailySigning> GetHistoryBetweenDates(DateTime startDate, DateTime endDate);
        DailySigning GetById(int id);
        DailySigning Add(DailySigning newDailySigning);
        bool Update(DailySigning dailySigning);
        bool Delete(int id);
        bool ValidateDalilySigning(DailySigning dailySigning);
        List<DailySigning> GetByUserId(DateTime startDate, DateTime endDate, int userId);
        List<DailySigning> GetByWorkId(DateTime startDate, DateTime endDate, int workId);
        List<DailySigning> GetByClientId(DateTime startDate, DateTime endDate, int clientId);
    }
}
