namespace SGDE.Domain.Repositories
{
    #region Using

    using Entities;
    using SGDE.Domain.Helpers;
    using System.Collections.Generic;

    #endregion

    public interface IDailySigningRepository
    {
        QueryResult<DailySigning> GetAll(int skip = 0, int take = 0, int userId = 0);
        DailySigning GetById(int id);
        DailySigning Add(DailySigning newDailySigning);
        bool Update(DailySigning dailySigning);
        bool Delete(int id);
        bool ValidateDalilySigning(DailySigning dailySigning);
        List<DailySigning> GetByUserId(string startDate, string endDate, int userId);
        List<DailySigning> GetByWorkId(string startDate, string endDate, int workId);
        List<DailySigning> GetByClientId(string startDate, string endDate, int clientId);
    }
}
