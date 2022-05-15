namespace SGDE.Domain.Supervisor
{
    #region Using

    using Converters;
    using Domain.Helpers;
    using Entities;
    using System;
    using System.Linq;
    using ViewModels;

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
                StartDate = newInvoiceViewModel.startDate,
                EndDate = newInvoiceViewModel.endDate,
                IssueDate = newInvoiceViewModel.issueDate,
                PayDate = newInvoiceViewModel.payDate,
                TaxBase = newInvoiceViewModel.taxBase,
                Iva = newInvoiceViewModel.iva,
                TypeInvoice = newInvoiceViewModel.typeInvoice,
                Retentions = newInvoiceViewModel.retentions,
                WorkId = newInvoiceViewModel.workId,
                ClientId = newInvoiceViewModel.clientId,
                UserId = newInvoiceViewModel.userId,
                InvoiceToCancelId = newInvoiceViewModel.invoiceToCancelId,
                WorkBudgetId = newInvoiceViewModel.workBudgetId
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

                StartDate = invoiceQueryViewModel.startDate,
                EndDate = invoiceQueryViewModel.endDate,
                IssueDate = invoiceQueryViewModel.issueDate,
                PayDate = invoiceQueryViewModel.payDate,
                WorkId = invoiceQueryViewModel.workId,
                ClientId = invoiceQueryViewModel.clientId,
                UserId = invoiceQueryViewModel.workerId,
                TypeInvoice = invoiceQueryViewModel.typeInvoice
            };

            var taxBase = 0.0;
            foreach (var detailInvoice in invoiceQueryViewModel.detailInvoice.OrderBy(x => x.id))
            {
                invoice.DetailsInvoice.Add(new DetailInvoice
                {
                    AddedDate = DateTime.Now,
                    ModifiedDate = null,

                    ServicesPerformed = detailInvoice.servicesPerformed,
                    Units = detailInvoice.units,
                    UnitsAccumulated = detailInvoice.unitsAccumulated,
                    PriceUnity = detailInvoice.priceUnity,
                    NameUnit = detailInvoice.nameUnit
                });
                taxBase += detailInvoice.units * detailInvoice.priceUnity;
            }
            invoice.TaxBase = taxBase;

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
            invoice.StartDate = invoiceViewModel.startDate;
            invoice.EndDate = invoiceViewModel.endDate;
            invoice.IssueDate = invoiceViewModel.issueDate;
            invoice.PayDate = invoiceViewModel.payDate;
            invoice.TaxBase = invoiceViewModel.taxBase;
            invoice.Iva = invoiceViewModel.iva;
            invoice.TypeInvoice = invoiceViewModel.typeInvoice;
            invoice.Retentions = invoiceViewModel.retentions;
            invoice.WorkId = invoiceViewModel.workId;
            invoice.ClientId = invoiceViewModel.clientId;
            invoice.UserId = invoiceViewModel.userId;
            invoice.InvoiceToCancelId = invoiceViewModel.invoiceToCancelId;
            invoice.WorkBudgetId = invoiceViewModel.workBudgetId;

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

            var invoiceNumber = _invoiceRepository.CountInvoicesInYear(DateTime.Now.Year);

            var newInvoice = new Invoice
            {
                Name = $"AB_{invoiceNumber:0000}_{DateTime.Now.Year.ToString().Substring(2, 2)}",
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
                Iva = invoiceParent.Iva,
                TypeInvoice = invoiceParent.TypeInvoice
            };

            var addInvoice = _invoiceRepository.Add(newInvoice);
            return InvoiceConverter.Convert(addInvoice);
        }

        public InvoiceViewModel GetPreviousInvoice(InvoiceViewModel invoiceViewModel)
        {
            var invoices = GetAllInvoice(0, 0, null, (int)invoiceViewModel.workId, 0);
            var invoice = invoices.Data
                .Where(x => x.endDate < invoiceViewModel.startDate)
                .OrderByDescending(x => x.startDate)
                .FirstOrDefault();

            if (invoice == null)
                throw new Exception("Esta factura no tiene facturas anteriores");

            var result = GetInvoiceById((int)invoice.id);
            if (result.detailInvoice != null)
            {
                foreach (var detailInvoce in result.detailInvoice)
                {
                    detailInvoce.unitsAccumulated = detailInvoce.units;
                    detailInvoce.units = 0;
                }
            }

            return result;
        }
    }
}
