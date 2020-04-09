namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using SGDE.Domain.Helpers;

    #endregion

    public class InvoiceRepository: IInvoiceRepository
    {
        private readonly EFContextMySQL _context;

        public InvoiceRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool InvoiceExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Invoice> GetAll(int skip = 0, int take = 0, string filter = null, int workId = 0, int clientId = 0)
        {
            List<Invoice> data = new List<Invoice>();

            if (workId == 0 && clientId == 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.InvoiceToCancel)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (workId != 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.InvoiceToCancel)
                    .Where(x => x.WorkId == workId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (workId == 0 && clientId != 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.InvoiceToCancel)
                    .Where(x => x.Work.ClientId == clientId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Work.Name?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Work.Client.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<Invoice>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<Invoice>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public Invoice GetById(int id)
        {
            return _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Invoices)
                    .Include(x => x.InvoiceToCancel)
                    .ThenInclude(x => x.DetailsInvoice)
                    .Include(x => x.InvoiceToCancel)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.DetailsInvoice)
                .FirstOrDefault(x => x.Id == id);
        }

        public Invoice Add(Invoice newInvoice)
        {
            _context.Invoice.Add(newInvoice);
            _context.SaveChanges();
            return newInvoice;
        }

        public Invoice AddInvoiceFromQuery(Invoice invoice)
        {
            var checkInvoice = CheckInvoice(invoice);

            if (checkInvoice == null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (invoice.WorkId != null)
                        {
                            var work = _context.Work.FirstOrDefault(x => x.Id == invoice.WorkId);
                            invoice.Iva = work != null ? !work.PassiveSubject : true;
                        }
                        else
                        {
                            invoice.Iva = true;
                        }

                        invoice.IvaTaxBase = invoice.Iva == true ? Math.Round((double)invoice.TaxBase * 0.21, 2) : 0;
                        invoice.Total = invoice.IvaTaxBase + (double)invoice.TaxBase;

                        var countInvoice = _context.Invoice.Count();
                        var invoiceNumber = countInvoice == 0 ? 1060 : (1060 + countInvoice);
                        invoiceNumber++;

                        invoice.InvoiceNumber = invoiceNumber;
                        invoice.Name = $"{invoiceNumber}/{invoice.StartDate.Year.ToString().Substring(2, 2)}";

                        _context.Invoice.Add(invoice);
                        _context.SaveChanges();

                        foreach (var detailInvoice in invoice.DetailsInvoice)
                        {
                            _context.DetailInvoice.Update(detailInvoice);
                            _context.SaveChanges();
                        }

                        transaction.Commit();
                        invoice.State = 1;

                        return invoice;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            if (checkInvoice != 0 && checkInvoice != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (invoice.WorkId != null)
                        {
                            var work = _context.Work.FirstOrDefault(x => x.Id == invoice.WorkId);
                            invoice.Iva = work != null ? !work.PassiveSubject : true;
                        }
                        else
                        {
                            invoice.Iva = true;
                        }

                        var findInvoice = _context.Invoice
                            .Include(x => x.DetailsInvoice)
                            .FirstOrDefault(x => x.Id == checkInvoice);
                        if (findInvoice == null)
                            throw new Exception("Factura no encontrada");

                        findInvoice.ModifiedDate = DateTime.Now;
                        findInvoice.IssueDate = invoice.IssueDate;

                        findInvoice.TaxBase = invoice.TaxBase;
                        findInvoice.IvaTaxBase = invoice.Iva == true ? Math.Round((double)invoice.TaxBase * 0.21, 2) : 0;
                        findInvoice.Total = findInvoice.IvaTaxBase + (double)findInvoice.TaxBase;

                        _context.Invoice.Update(findInvoice);
                        _context.SaveChanges();

                        _context.DetailInvoice.RemoveRange(findInvoice.DetailsInvoice);
                        _context.SaveChanges();

                        foreach (var detailInvoice in invoice.DetailsInvoice)
                        {
                            detailInvoice.InvoiceId = findInvoice.Id;
                            _context.DetailInvoice.Add(detailInvoice);
                            _context.SaveChanges();
                        }

                        transaction.Commit();
                        findInvoice.State = 2;

                        return findInvoice;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            if (checkInvoice == 0)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (invoice.WorkId != null)
                        {
                            var work = _context.Work.FirstOrDefault(x => x.Id == invoice.WorkId);
                            invoice.Iva = work != null ? !work.PassiveSubject : true;
                        }
                        else
                        {
                            invoice.Iva = true;
                        }

                        var findInvoice = _context.Invoice
                             .Include(x => x.DetailsInvoice)
                             .FirstOrDefault(x =>
                                        x.WorkId == invoice.WorkId &&
                                        x.StartDate == invoice.StartDate &&
                                        x.EndDate == invoice.EndDate);
                        if (findInvoice == null)
                            throw new Exception("Factura no encontrada");

                        _context.DetailInvoice.RemoveRange(findInvoice.DetailsInvoice);
                        _context.SaveChanges();

                        findInvoice.TaxBase = invoice.TaxBase;
                        findInvoice.IvaTaxBase = invoice.Iva == true ? Math.Round((double)invoice.TaxBase * 0.21, 2) : 0;
                        findInvoice.Total = findInvoice.IvaTaxBase + (double)findInvoice.TaxBase;

                        foreach (var detailInvoice in invoice.DetailsInvoice)
                        {
                            detailInvoice.InvoiceId = findInvoice.Id;
                            _context.DetailInvoice.Add(detailInvoice);
                            _context.SaveChanges();
                        }

                        transaction.Commit();
                        findInvoice.State = 3;

                        return findInvoice;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            var existInvoice = _context.Invoice
                .Include(x => x.DetailsInvoice)
                .FirstOrDefault(x =>
                    x.WorkId == invoice.WorkId &&
                    x.StartDate == invoice.StartDate &&
                    x.EndDate == invoice.EndDate);

            existInvoice.State = 3;
            return existInvoice;
        }

        public bool Update(Invoice invoice)
        {
            if (!InvoiceExists(invoice.Id))
                return false;

            _context.Invoice.Update(invoice);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!InvoiceExists(id))
                return false;

            var toRemove = _context.Invoice.Find(id);
            _context.Invoice.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }


        public int? CheckInvoice(Invoice newInvoice)
        {
            var invoice = _context.Invoice
                                    .FirstOrDefault(x =>
                                        x.WorkId == newInvoice.WorkId &&
                                        x.StartDate == newInvoice.StartDate &&
                                        x.EndDate == newInvoice.EndDate);
            if (invoice == null)
                return null;

            if (newInvoice.TaxBase == invoice.TaxBase &&
                newInvoice.Iva == invoice.Iva &&
                newInvoice.Total == invoice.Total)
            {
                return 0;
            }

            return invoice.Id;
        }

        public int CountInvoices()
        {
            return _context.Invoice.Count();
        }
    }
}
