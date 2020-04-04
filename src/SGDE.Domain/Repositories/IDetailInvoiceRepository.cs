namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IDetailInvoiceRepository
    {
        List<DetailInvoice> GetAll();
        DetailInvoice GetById(int id);
        DetailInvoice Add(DetailInvoice newDetailInvoice);
        bool Update(DetailInvoice detailInvoice);
        bool Delete(int id);
    }
}
