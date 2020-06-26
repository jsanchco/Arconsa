namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using System.Collections.Generic;

    #endregion

    public interface IUserHiringRepository
    {
        QueryResult<UserHiring> GetAll(int skip = 0, int take = 0, string filter = null, int userId = 0, int workId = 0);
        List<UserHiring> GetOpen();
        UserHiring GetById(int id);
        UserHiring Add(UserHiring newUserHiring);
        bool Update(UserHiring userHiring);
        bool Delete(int id);
        bool AssignWorkers(List<int> listUserId, int workId);
        bool IsProfessionInClient(int? professionId, int workId = 0, int clientId = 0);
        UserHiring GetByWorkAndStartDateNull(int workId);
        UserHiring GetByWorkerAndEndDateNull(int workerId);
    }
}
