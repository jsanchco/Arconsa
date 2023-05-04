using SGDE.Domain.Entities;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IEnterpriseRepository
    {
        List<Enterprise> GetAll();
        List<Enterprise> GetByUserId(int userId);
        Enterprise GetById(int id);
        Enterprise Add(Enterprise newEnterprise);
        bool Update(Enterprise enterprise);
        bool Delete(int id);
    }
}
