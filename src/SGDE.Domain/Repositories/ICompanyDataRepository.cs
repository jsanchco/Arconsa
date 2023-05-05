using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface ICompanyDataRepository
    {
        QueryResult<CompanyData> GetAll(int skip = 0, int take = 0, int enterpriseId = 0, string filter = null);
        CompanyData GetById(int id);
        CompanyData Add(CompanyData newCompanyData);
        bool Update(CompanyData companyData);
        bool Delete(int id);
    }
}
