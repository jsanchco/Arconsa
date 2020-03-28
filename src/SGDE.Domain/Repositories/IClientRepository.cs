namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using System.Collections.Generic;

    #endregion

    public interface IClientRepository
    {
        QueryResult<Client> GetAll(int skip = 0, int take = 0, string filter = null);
        Client GetById(int id);
        Client Add(Client newClient);
        bool Update(Client client);
        bool Delete(int id);
    }
}
