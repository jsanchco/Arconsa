namespace SGDE.Domain.Converters
{
    #region Using

    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    #endregion

    public class InvoiceConverter
    {
        public static InvoiceViewModel Convert(Invoice invoice)
        {
            if (invoice == null)
                return null;

            var invoiceViewModel = new InvoiceViewModel
            {
                id = invoice.Id,
                addedDate = invoice.AddedDate,
                modifiedDate = invoice.ModifiedDate,
                iPAddress = invoice.IPAddress,

                invoiceNumber = invoice.InvoiceNumber,
                name = invoice.Name,
                startDate = invoice.StartDate,
                endDate = invoice.EndDate,
                issueDate = invoice.IssueDate,
                payDate = invoice.PayDate,
                expirationDate = invoice.Work.Client.ExpirationDays != 0 ? (DateTime?)invoice.IssueDate.AddDays(invoice.Work.Client.ExpirationDays) : null,
                //taxBase = Math.Round(invoice.TaxBase, 2),
                iva = invoice.Iva,
                typeInvoice = invoice.TypeInvoice,
                //retentions = invoice.Work.InvoiceToOrigin == true ? ((double)invoice.TaxBase * (double)invoice.Work.PercentageRetention) : 0,
                workId = invoice.WorkId,
                workName = invoice.Work.Name,
                clientId = invoice.Work.ClientId,
                clientName = invoice.Work.Client.Name,
                userId = invoice.UserId,
                userName = invoice.UserId == null ? null : $"{invoice.User.Name} {invoice.User.Surname}",
                invoiceToCancelId = invoice.InvoiceToCancelId,
                invoiceToCancelName = invoice.InvoiceToCancel?.Name,
                workBudgetId = invoice.WorkBudgetId,
                workBudgetName = invoice.WorkBudget?.Name,
                ivaTaxBase = invoice.IvaTaxBase,
                taxBase = invoice.TaxBase,
                totalPayment = invoice.TotalPayment,

                //detailInvoice = DetailInvoiceConverter.ConvertList(invoice.DetailsInvoice)
            };

            invoiceViewModel.retentions = invoice.Work.InvoiceToOrigin == true ? (invoiceViewModel.taxBase) * (double)invoice.Work.PercentageRetention : 0;
            invoiceViewModel.total = Math.Round(invoiceViewModel.taxBase + invoiceViewModel.ivaTaxBase - invoiceViewModel.retentions, 2);

            return invoiceViewModel;
        }

        public static List<InvoiceViewModel> ConvertList(IEnumerable<Invoice> invoices)
        {
            return invoices?.Select(invoice =>
            {
                var model = new InvoiceViewModel
                {
                    id = invoice.Id,
                    addedDate = invoice.AddedDate,
                    modifiedDate = invoice.ModifiedDate,
                    iPAddress = invoice.IPAddress,

                    invoiceNumber = invoice.InvoiceNumber,
                    name = invoice.Name,
                    startDate = invoice.StartDate,
                    endDate = invoice.EndDate,
                    issueDate = invoice.IssueDate,
                    payDate = invoice.PayDate,
                    expirationDate = invoice.Work.Client.ExpirationDays != 0 ? (DateTime?)invoice.IssueDate.AddDays(invoice.Work.Client.ExpirationDays) : null,
                    //taxBase = Math.Round(invoice.TaxBase, 2),
                    iva = invoice.Iva,
                    typeInvoice = invoice.TypeInvoice,
                    //retentions = invoice.Work.InvoiceToOrigin == true ? (invoice.DetailsInvoice.Sum(x => x.Units) * (double)invoice.Work.PercentageRetention) : 0,
                    workId = invoice.WorkId,
                    workName = invoice.Work.Name,
                    clientId = invoice.Work.ClientId,
                    clientName = invoice.Work.Client.Name,
                    userId = invoice.UserId,
                    userName = invoice.UserId == null ? null : $"{invoice.User.Name} {invoice.User.Surname}",
                    invoiceToCancelId = invoice.InvoiceToCancelId,
                    invoiceToCancelName = invoice.InvoiceToCancel?.Name,
                    workBudgetId = invoice.WorkBudgetId,
                    workBudgetName = invoice.WorkBudget?.Name,
                    ivaTaxBase = invoice.IvaTaxBase,
                    taxBase = invoice.TaxBase,
                    totalPayment = invoice.TotalPayment,

                    //detailInvoice = DetailInvoiceConverter.ConvertList(invoice.DetailsInvoice)
                };
                model.retentions = invoice.Work.InvoiceToOrigin == true ? model.taxBase * (double)invoice.Work.PercentageRetention : 0;
                model.total = Math.Round(model.taxBase + model.ivaTaxBase - model.retentions, 2);

                return model;
            })
                .ToList();
        }
    }
}
