using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public class InvoicePaymentHistoryConverter
    {
        public static InvoicePaymentHistoryViewModel Convert(InvoicePaymentHistory invoicePaymentHistory)
        {
            if (invoicePaymentHistory == null)
                return null;

            var invoicePaymentHistoryViewModel = new InvoicePaymentHistoryViewModel
            {
                id = invoicePaymentHistory.Id,
                addedDate = invoicePaymentHistory.AddedDate,
                modifiedDate = invoicePaymentHistory.ModifiedDate,
                iPAddress = invoicePaymentHistory.IPAddress,

                datePayment = invoicePaymentHistory.DatePayment,
                amount = invoicePaymentHistory.Amount,
                observations = invoicePaymentHistory.Observations,
                invoiceId = invoicePaymentHistory.InvoiceId,
            };

            return invoicePaymentHistoryViewModel;
        }

        public static List<InvoicePaymentHistoryViewModel> ConvertList(IEnumerable<InvoicePaymentHistory> invoicePaymentsHistory)
        {
            return invoicePaymentsHistory?.Select(invoicePaymentHistory =>
            {
                var model = new InvoicePaymentHistoryViewModel
                {
                    id = invoicePaymentHistory.Id,
                    addedDate = invoicePaymentHistory.AddedDate,
                    modifiedDate = invoicePaymentHistory.ModifiedDate,
                    iPAddress = invoicePaymentHistory.IPAddress,

                    datePayment = invoicePaymentHistory.DatePayment,
                    amount = invoicePaymentHistory.Amount,
                    observations = invoicePaymentHistory.Observations,
                    invoiceId = invoicePaymentHistory.InvoiceId,
                };
                return model;
            })
                .ToList();
        }
    }
}
