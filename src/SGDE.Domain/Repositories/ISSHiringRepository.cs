using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface ISSHiringRepository
    {
        QueryResult<SSHiring> GetAll(int skip = 0, int take = 0, int userId = 0);
        SSHiring GetById(int id);
        SSHiring Add(SSHiring newSSHiring);
        bool Update(SSHiring sSHiring);
        bool Delete(int id);
    }
}
