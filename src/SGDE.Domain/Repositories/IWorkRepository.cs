namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IWorkRepository
    {
        List<Work> GetAll();
        Work GetById(int id);
        Work Add(Work newWork);
        bool Update(Work work);
        bool Delete(int id);
    }
}
