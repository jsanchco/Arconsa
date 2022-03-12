namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IDetailInvoiceRepository
    {
        List<DetailInvoice> GetAll(int invoiceId = 0, bool previousInvoice = false);
        DetailInvoice GetById(int id);
        DetailInvoice Add(DetailInvoice newDetailInvoice);
        bool Update(DetailInvoice detailInvoice);
        bool Delete(int id);
        List<DetailInvoice> UpdateFromPreviousInvoice(int invoiceId);
        List<DetailInvoice> UpdateFromWork(int invoiceId, List<DetailInvoice> detailsInvoice);
        List<DetailInvoice> UpdateToEmptyDetails(int invoiceId);
        List<DetailInvoice> GetAllWithIncludes();
    }
}
