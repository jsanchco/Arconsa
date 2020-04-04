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
                startDate = invoice.StartDate.ToString("dd/MM/yyyy"),
                endDate = invoice.EndDate.ToString("dd/MM/yyyy"),
                taxBase = System.Convert.ToDouble(invoice.TaxBase),
                iva = System.Convert.ToDouble(invoice.Iva),
                total = System.Convert.ToDouble(invoice.Total),
                retentions = invoice.Retentions,
                workId = invoice.WorkId,
                workName = invoice.Work.Name,
                clientId = invoice.Work.ClientId,
                clientName = invoice.Work.Client.Name
            };

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
                    startDate = invoice.StartDate.ToString("dd/MM/yyyy"),
                    endDate = invoice.EndDate.ToString("dd/MM/yyyy"),
                    taxBase = System.Convert.ToDouble(invoice.TaxBase),
                    iva = System.Convert.ToDouble(invoice.Iva),
                    total = System.Convert.ToDouble(invoice.Total),
                    retentions = invoice.Retentions,
                    workId = invoice.WorkId,
                    workName = invoice.Work.Name,
                    clientId = invoice.Work.ClientId,
                    clientName = invoice.Work.Client.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
