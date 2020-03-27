namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IUserHiringRepository
    {
        List<UserHiring> GetAll(int userId = 0, int workId = 0);
        List<UserHiring> GetOpen();
        UserHiring GetById(int id);
        UserHiring Add(UserHiring newUserHiring);
        bool Update(UserHiring userHiring);
        bool Delete(int id);
        bool AssignWorkers(List<int> listUserId, int workId);
        bool IsProfessionInClient(int? professionId, int workId = 0, int clientId = 0);
    }
}
