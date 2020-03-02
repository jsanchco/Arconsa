namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface ITypeClientRepository
    {
        List<TypeClient> GetAll();
        TypeClient GetById(int id);
        TypeClient Add(TypeClient newTypeClient);
        bool Update(TypeClient typeClient);
        bool Delete(int id);
    }
}
