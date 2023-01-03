using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public List<InvoicePaymentHistoryViewModel> GetAllInvoicePaymentHistory(int invoiceId = 0)
        {
            return InvoicePaymentHistoryConverter.ConvertList(_invoicePaymentHistoryRepository.GetAll(invoiceId));
        }

        public InvoicePaymentHistoryViewModel GetInvoicePaymentHistoryById(int id)
        {
            var invoicePaymentHistoryViewModel = InvoicePaymentHistoryConverter.Convert(_invoicePaymentHistoryRepository.GetById(id));

            return invoicePaymentHistoryViewModel;
        }

        public InvoicePaymentHistoryViewModel AddInvoicePaymentHistory(InvoicePaymentHistoryViewModel newInvoicePaymentHistoryViewModel)
        {
            var invoicePaymentHistory = new InvoicePaymentHistory
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newInvoicePaymentHistoryViewModel.iPAddress,

                DatePayment = newInvoicePaymentHistoryViewModel.datePayment,
                Amount = newInvoicePaymentHistoryViewModel.amount,
                Observations = newInvoicePaymentHistoryViewModel.observations,

                InvoiceId = newInvoicePaymentHistoryViewModel.invoiceId
            };

            _invoicePaymentHistoryRepository.Add(invoicePaymentHistory);
            return newInvoicePaymentHistoryViewModel;
        }

        public bool UpdateInvoicePaymentHistory(InvoicePaymentHistoryViewModel invoicePaymentHistoryViewModel)
        {
            if (invoicePaymentHistoryViewModel.id == null)
                return false;

            var invoicePaymentHistory = _invoicePaymentHistoryRepository.GetById((int)invoicePaymentHistoryViewModel.id);

            if (invoicePaymentHistory == null) return false;

            invoicePaymentHistory.ModifiedDate = DateTime.Now;
            invoicePaymentHistory.IPAddress = invoicePaymentHistoryViewModel.iPAddress;

            invoicePaymentHistory.DatePayment = invoicePaymentHistoryViewModel.datePayment;
            invoicePaymentHistory.Amount = invoicePaymentHistoryViewModel.amount;
            invoicePaymentHistory.Observations = invoicePaymentHistoryViewModel.observations;

            invoicePaymentHistory.InvoiceId = invoicePaymentHistoryViewModel.invoiceId;

            return _invoicePaymentHistoryRepository.Update(invoicePaymentHistory);
        }

        public bool DeleteInvoicePaymentHistory(int id)
        {
            return _invoicePaymentHistoryRepository.Delete(id);
        }
    }
}
