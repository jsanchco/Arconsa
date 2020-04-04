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
                TaxBase =  System.Convert.ToDecimal(newInvoiceViewModel.taxBase),
                Iva = System.Convert.ToDecimal(newInvoiceViewModel.iva),
                Total = System.Convert.ToDecimal(newInvoiceViewModel.total),
                Retentions = newInvoiceViewModel.retentions,
                WorkId = newInvoiceViewModel.workId
            };

            var checkInvoice = _invoiceRepository.CheckInvoice(invoice);
            if (checkInvoice == null)
                _invoiceRepository.Add(invoice);

            if (checkInvoice != null && checkInvoice != 0)
            {
                var updateInvoice =_invoiceRepository.GetById((int)checkInvoice);
                updateInvoice.ModifiedDate = DateTime.Now;
                updateInvoice.TaxBase = invoice.TaxBase;
                updateInvoice.Iva = invoice.Iva;
                updateInvoice.Total = invoice.Total;
                updateInvoice.Retentions = invoice.Retentions;
                updateInvoice.WorkId = invoice.WorkId;

                _invoiceRepository.Update(updateInvoice);
            }

            return newInvoiceViewModel;
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
            invoice.TaxBase = System.Convert.ToDecimal(invoiceViewModel.taxBase);
            invoice.Iva = System.Convert.ToDecimal(invoiceViewModel.iva);
            invoice.Total = System.Convert.ToDecimal(invoiceViewModel.total);
            invoice.Retentions = invoiceViewModel.retentions;
            invoice.WorkId = invoiceViewModel.workId;

            return _invoiceRepository.Update(invoice);
        }

        public bool DeleteInvoice(int id)
        {
            return _invoiceRepository.Delete(id);
        }
    }
}
