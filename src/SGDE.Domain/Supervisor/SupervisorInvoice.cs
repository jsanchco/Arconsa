namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using Domain.Helpers;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<InvoiceViewModel> GetAllInvoice(int skip = 0, int take = 0, string filter = null, int workId = 0, int clientId = 0)
        {
            var queryResult = _invoiceRepository.GetAll(skip, take, filter, workId, clientId);
            return new QueryResult<InvoiceViewModel>
            {
                Data = InvoiceConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public InvoiceViewModel GetInvoiceById(int id)
        {
            var invoiceViewModel = InvoiceConverter.Convert(_invoiceRepository.GetById(id));

            return invoiceViewModel;
        }

        public InvoiceViewModel AddInvoice(InvoiceViewModel newInvoiceViewModel)
        {
            var invoice = new Invoice
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newInvoiceViewModel.iPAddress,

                InvoiceNumber = newInvoiceViewModel.invoiceNumber,
                Name = newInvoiceViewModel.name,
                StartDate = DateTime.ParseExact(newInvoiceViewModel.startDate, "dd/MM/yyyy", null),
                EndDate = DateTime.ParseExact(newInvoiceViewModel.endDate, "dd/MM/yyyy", null),
                IssueDate = DateTime.ParseExact(newInvoiceViewModel.issueDate, "dd/MM/yyyy", null),
                TaxBase = (decimal)newInvoiceViewModel.taxBase,
                Iva = newInvoiceViewModel.iva,
                Retentions = newInvoiceViewModel.retentions,
                WorkId = newInvoiceViewModel.workId,
                ClientId = newInvoiceViewModel.clientId,
                UserId = newInvoiceViewModel.userId,
                InvoiceToCancelId = newInvoiceViewModel.invoiceToCancelId
            };

            _invoiceRepository.Add(invoice);
            return newInvoiceViewModel;
        }

        public Invoice GetInvoice(int invoiceId)
        {
            var invoice = _invoiceRepository.GetById(invoiceId);
            if (invoice.InvoiceToCancelId != null)
                invoice.DetailsInvoice = invoice.InvoiceToCancel.DetailsInvoice;

            return invoice;
        }

        public Invoice AddInvoiceFromQuery(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var invoice = new Invoice
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,

                StartDate = DateTime.ParseExact(invoiceQueryViewModel.startDate, "dd/MM/yyyy", null),
                EndDate = DateTime.ParseExact(invoiceQueryViewModel.endDate, "dd/MM/yyyy", null),
                IssueDate = DateTime.ParseExact(invoiceQueryViewModel.issueDate, "dd/MM/yyyy", null),
                WorkId = invoiceQueryViewModel.workId,
                ClientId = invoiceQueryViewModel.clientId,
                UserId = invoiceQueryViewModel.workerId
            };

            var taxBase = 0.0;
            foreach (var detailInvoice in invoiceQueryViewModel.detailInvoice)
            {
                invoice.DetailsInvoice.Add(new DetailInvoice
                {
                    AddedDate = DateTime.Now,
                    ModifiedDate = null,

                    ServicesPerformed = detailInvoice.servicesPerformed,
                    Units = (decimal)detailInvoice.units,
                    PriceUnity = (decimal)detailInvoice.priceUnity,
                    NameUnit = detailInvoice.nameUnit
                });
                taxBase += detailInvoice.units * detailInvoice.priceUnity;
            }
            invoice.TaxBase = (decimal)taxBase;

            return _invoiceRepository.AddInvoiceFromQuery(invoice);
        }

        public bool UpdateInvoice(InvoiceViewModel invoiceViewModel)
        {
            if (invoiceViewModel.id == null)
                return false;

            var invoice = _invoiceRepository.GetById((int)invoiceViewModel.id);

            if (invoice == null) return false;

            invoice.ModifiedDate = DateTime.Now;
            invoice.IPAddress = invoiceViewModel.iPAddress;

            invoice.InvoiceNumber = invoiceViewModel.invoiceNumber;
            invoice.Name = invoiceViewModel.name;
            invoice.StartDate = DateTime.ParseExact(invoiceViewModel.startDate, "dd/MM/yyyy", null);
            invoice.EndDate = DateTime.ParseExact(invoiceViewModel.endDate, "dd/MM/yyyy", null);
            invoice.IssueDate = DateTime.ParseExact(invoiceViewModel.issueDate, "dd/MM/yyyy", null);
            invoice.TaxBase = (decimal)invoiceViewModel.taxBase;
            invoice.Iva = invoiceViewModel.iva;
            invoice.Retentions = invoiceViewModel.retentions;
            invoice.WorkId = invoiceViewModel.workId;
            invoice.ClientId = invoiceViewModel.clientId;
            invoice.UserId = invoiceViewModel.userId;
            invoice.InvoiceToCancelId = invoiceViewModel.invoiceToCancelId;

            return _invoiceRepository.Update(invoice);
        }

        public bool DeleteInvoice(int id)
        {
            return _invoiceRepository.Delete(id);
        }

        public InvoiceResponseViewModel PrintInvoice(int invoiceId)
        {
            var generateInvoice = new PrintInvoice(this, invoiceId);
            generateInvoice.Print();
            return generateInvoice._invoiceResponseViewModel;
        }

        public InvoiceViewModel BillPayment(int invoiceId)
        {
            var invoiceParent = _invoiceRepository.GetById(invoiceId);
            if (invoiceParent == null)
                throw new Exception("Factura no encontrada");

            if (invoiceParent.InvoiceToCancelId != null)
                throw new Exception("No de puede anular una Factura ya Anulada");

            var countInvoice = _invoiceRepository.CountInvoices();
            var invoiceNumber = countInvoice == 0 ? 1063 : (1063 + countInvoice);
            invoiceNumber++;

            var newInvoice = new Invoice
            {
                Name = $"AB_{invoiceParent.Name}",
                InvoiceNumber = invoiceNumber,
                InvoiceToCancelId = invoiceId,
                IssueDate = DateTime.Now,
                TaxBase = -invoiceParent.TaxBase,
                WorkId = invoiceParent.WorkId,
                ClientId = invoiceParent.ClientId,
                UserId = invoiceParent.UserId,
                StartDate = invoiceParent.StartDate,
                EndDate = invoiceParent.EndDate,
                Retentions = invoiceParent.Retentions,
                AddedDate = DateTime.Now,
                Iva = invoiceParent.Iva
            };

            return InvoiceConverter.Convert(_invoiceRepository.Add(newInvoice));
        }
    }
}
