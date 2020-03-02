namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface ITypeDocumentRepository
    {
        List<TypeDocument> GetAll();
        TypeDocument GetById(int id);
        TypeDocument Add(TypeDocument newTypeDocument);
        bool Update(TypeDocument typeDocument);
        bool Delete(int id);
    }
}
