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
                taxBase = (double)invoice.TaxBase,
                iva = invoice.Iva,
                typeInvoice = invoice.TypeInvoice,
                retentions = invoice.Work.InvoiceToOrigin == true ? (decimal)((double)invoice.TaxBase * (double)invoice.Work.PercentageRetention) : 0,
                workId = invoice.WorkId,
                workName = invoice.Work.Name,
                clientId = invoice.ClientId == null ? invoice.Work.ClientId : invoice.Client.Id,
                clientName = invoice.ClientId == null ? invoice.Work.Client.Name : invoice.Client.Name,
                userId = invoice.UserId,
                userName = invoice.UserId == null ? null : $"{invoice.User.Name} {invoice.User.Surname}",
                invoiceToCancelId = invoice.InvoiceToCancelId,
                invoiceToCancelName = invoice.InvoiceToCancel?.Name,
                detailInvoice = DetailInvoiceConverter.ConvertList(invoice.DetailsInvoice),
                workBudgetId = invoice.WorkBudgetId,
                workBudgetName = invoice.WorkBudget?.Name
            };
            if (invoice.Iva == true)
            {
                invoiceViewModel.ivaTaxBase = Math.Round(invoiceViewModel.taxBase * (double)invoice.Work.PercentageIVA, 2);
                invoiceViewModel.total = Math.Round(invoiceViewModel.taxBase + invoiceViewModel.ivaTaxBase);
            }
            else
            {
                invoiceViewModel.ivaTaxBase = 0;
                invoiceViewModel.total = Math.Round(invoiceViewModel.taxBase);
            }

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
                    taxBase = (double)invoice.TaxBase,
                    iva = invoice.Iva,
                    typeInvoice = invoice.TypeInvoice,
                    retentions = invoice.Work.InvoiceToOrigin == true ? (decimal)((double)invoice.TaxBase * (double)invoice.Work.PercentageRetention) : 0,
                    workId = invoice.WorkId,
                    workName = invoice.Work.Name,
                    clientId = invoice.ClientId == null ? invoice.Work.ClientId : invoice.Client.Id,
                    clientName = invoice.ClientId == null ? invoice.Work.Client.Name : invoice.Client.Name,
                    userId = invoice.UserId,
                    userName = invoice.UserId == null ? null : $"{invoice.User.Name} {invoice.User.Surname}",
                    invoiceToCancelId = invoice.InvoiceToCancelId,
                    invoiceToCancelName = invoice.InvoiceToCancel?.Name,
                    workBudgetId = invoice.WorkBudgetId,
                    workBudgetName = invoice.WorkBudget?.Name
                };
                if (invoice.Iva == true)
                {
                    model.ivaTaxBase = Math.Round(model.taxBase * (double)invoice.Work.PercentageIVA, 2);
                    model.total = Math.Round(model.taxBase + model.ivaTaxBase, 2);
                }
                else
                {
                    model.ivaTaxBase = 0;
                    model.total = Math.Round(model.taxBase, 2);
                }

                return model;
            })
                .ToList();
        }
    }
}
