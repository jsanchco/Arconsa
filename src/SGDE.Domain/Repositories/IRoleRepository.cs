namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IRoleRepository
    {
        List<Role> GetAll();
        Role GetById(int id);
        Role Add(Role newRole);
        bool Update(Role role);
        bool Delete(int id);
    }
}
