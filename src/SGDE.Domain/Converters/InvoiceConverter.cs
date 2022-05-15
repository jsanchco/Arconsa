namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;
    using System;

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
                retentions = invoice.Work.InvoiceToOrigin == true ? ((double)invoice.TaxBase * (double)invoice.Work.PercentageRetention) : 0,
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
                detailInvoice = invoice.InvoiceToCancelId == null ?
                        DetailInvoiceConverter.ConvertList(invoice.DetailsInvoice) :
                        DetailInvoiceConverter.ConvertList(invoice.InvoiceToCancel.DetailsInvoice, true)
            };

            invoiceViewModel.taxBase = Math.Round(invoiceViewModel.detailInvoice.Sum(x => x.amountUnits), 2);
            invoiceViewModel.ivaTaxBase = Math.Round(invoiceViewModel.detailInvoice.Sum(x => x.amountUnits * x.iva), 2);
            invoiceViewModel.total = Math.Round(invoiceViewModel.taxBase + invoiceViewModel.ivaTaxBase, 2);
            
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
                    retentions = invoice.Work.InvoiceToOrigin == true ? ((double)invoice.TaxBase * (double)invoice.Work.PercentageRetention) : 0,
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
                    detailInvoice = invoice.InvoiceToCancelId == null ? 
                        DetailInvoiceConverter.ConvertList(invoice.DetailsInvoice) :
                        DetailInvoiceConverter.ConvertList(invoice.InvoiceToCancel.DetailsInvoice, true)
                };
                model.taxBase = Math.Round(model.detailInvoice.Sum(x => x.amountUnits), 2);
                model.ivaTaxBase = Math.Round(model.detailInvoice.Sum(x => x.amountUnits * x.iva), 2);
                model.total = Math.Round(model.taxBase + model.ivaTaxBase, 2);

                return model;
            })
                .ToList();
        }
    }
}
