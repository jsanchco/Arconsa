namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IWorkRepository
    {
        List<Work> GetAll(int skip = 0, int take = 0, string filter = null, int clientId = 0);
        Work GetById(int id);
        Work Add(Work newWork);
        bool Update(Work work);
        bool Delete(int id);
    }
}
