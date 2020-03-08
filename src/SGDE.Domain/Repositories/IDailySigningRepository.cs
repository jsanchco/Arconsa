namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IDailySigningRepository
    {
        List<DailySigning> GetAll();
        DailySigning GetById(int id);
        DailySigning Add(DailySigning newDailySigning);
        bool Update(DailySigning dailySigning);
        bool Delete(int id);
        bool ValidateDalilySigning(DailySigning dailySigning);
    }
}
