using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface IDetailEmbargoRepository
    {
        QueryResult<DetailEmbargo> GetAll(int skip = 0, int take = 0, int embargoId = 0);
        DetailEmbargo GetById(int id);
        DetailEmbargo Add(DetailEmbargo newDetailEmbargo, bool isPaid);
        bool Update(DetailEmbargo detailEmbargo, bool isPaid);
        bool Delete(int id);
    }
}
