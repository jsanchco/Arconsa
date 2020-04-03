namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;

    #endregion

    public interface IInvoiceRepository
    {
        QueryResult<Invoice> GetAll(int skip = 0, int take = 0, string filter = null, int workId = 0, int clientId = 0);
        Invoice GetById(int id);
        Invoice Add(Invoice newInvoice);
        bool Update(Invoice invoice);
        bool Delete(int id);

        int? CheckInvoice(Invoice newInvoice);
    }
}
