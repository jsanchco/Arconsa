namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IPromoterRepository
    {
        List<Promoter> GetAll();
        Promoter GetById(int id);
        Promoter Add(Promoter newPromoter);
        bool Update(Promoter promoter);
        bool Delete(int id);
    }
}
