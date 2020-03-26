namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;

    #endregion

    public interface IProfessionInClientRepository
    {
        QueryResult<ProfessionInClient> GetAll(int skip = 0, int take = 0, string filter = null, int professionId = 0, int clientId = 0);
        ProfessionInClient GetById(int id);
        ProfessionInClient Add(ProfessionInClient newProfessionInClient);
        bool Update(ProfessionInClient professionInClient);
        bool Delete(int id);
    }
}
