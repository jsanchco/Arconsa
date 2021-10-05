namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;

    #endregion

    public interface IWorkRepository
    {
        QueryResult<Work> GetAll(int skip = 0, int take = 0, string filter = null, int clientId = 0, bool showCloseWorks = true);
        Work GetById(int id);
        Work Add(Work newWork);
        bool Update(Work work);
        bool Delete(int id);
    }
}
