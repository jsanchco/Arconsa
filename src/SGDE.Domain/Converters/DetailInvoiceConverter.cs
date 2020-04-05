namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class DetailInvoiceConverter 
    {
        public static DetailInvoiceViewModel Convert(DetailInvoice detailInvoice)
        {
            if (detailInvoice == null)
                return null;

            var detailInvoiceViewModel = new DetailInvoiceViewModel
            {
                id = detailInvoice.Id,
                addedDate = detailInvoice.AddedDate,
                modifiedDate = detailInvoice.ModifiedDate,
                iPAddress = detailInvoice.IPAddress,

                invoiceId = detailInvoice.InvoiceId,
                servicesPerformed = detailInvoice.ServicesPerformed,
                units = (double)detailInvoice.Units,
                priceUnity = (double)detailInvoice.PriceUnity,
                nameUnit = detailInvoice.NameUnit
            };

            return detailInvoiceViewModel;
        }

        public static List<DetailInvoiceViewModel> ConvertList(IEnumerable<DetailInvoice> detailInvoices)
        {
            return detailInvoices?.Select(detailInvoice =>
            {
                var model = new DetailInvoiceViewModel
                {
                    id = detailInvoice.Id,
                    addedDate = detailInvoice.AddedDate,
                    modifiedDate = detailInvoice.ModifiedDate,
                    iPAddress = detailInvoice.IPAddress,

                    invoiceId = detailInvoice.InvoiceId,
                    servicesPerformed = detailInvoice.ServicesPerformed,
                    units = (double)detailInvoice.Units,
                    priceUnity = (double)detailInvoice.PriceUnity,
                    nameUnit = detailInvoice.NameUnit
                };
                return model;
            })
                .ToList();
        }
    }
}
