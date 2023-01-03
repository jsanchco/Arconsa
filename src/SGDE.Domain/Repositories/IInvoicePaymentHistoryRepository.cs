using SGDE.Domain.Entities;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IInvoicePaymentHistoryRepository
    {
        List<InvoicePaymentHistory> GetAll(int invoiceId = 0);
        InvoicePaymentHistory GetById(int id);
        InvoicePaymentHistory Add(InvoicePaymentHistory newInvoicePaymentHistory);
        bool Update(InvoicePaymentHistory invoicePaymentHistory);
        bool Delete(int id);
    }
}
