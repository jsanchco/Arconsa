using SGDE.Domain.Entities;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IUserProfessionRepository
    {
        List<UserProfession> GetAll(int userId);
        UserProfession GetById(int id);
        UserProfession Add(UserProfession newUserProfession);
        bool Update(UserProfession userProfession);
        bool Delete(int userProfessionId);
        bool Delete(int userId, int professionId);
    }
}
