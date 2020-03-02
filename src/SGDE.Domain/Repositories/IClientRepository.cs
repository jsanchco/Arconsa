namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IClientRepository
    {
        List<Client> GetAll();
        Client GetById(int id);
        Client Add(Client newClient);
        bool Update(Client client);
        bool Delete(int id);
    }
}
