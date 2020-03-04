namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IUserDocumentRepository
    {
        List<UserDocument> GetAll(int userId);
        UserDocument GetById(int id);
        UserDocument Add(UserDocument newUserDocument);
        bool Update(UserDocument userDocument);
        bool Delete(int id);
    }
}
