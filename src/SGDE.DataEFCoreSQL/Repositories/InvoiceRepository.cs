﻿namespace SGDE.DataEFCoreSQL.Repositories
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

    public class InvoiceRepository : IInvoiceRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public InvoiceRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
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
                    .Include(x => x.Work)
                    .ThenInclude(x => x.WorkBudgets)
                    .ToList()
                    .OrderByDescending(x => x.KeyOrder)
                    .ToList();
            }

            if (workId != 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.InvoiceToCancel)
                    .Include(x => x.Work)
                    .ThenInclude(x => x.WorkBudgets)
                    .Where(x => x.WorkId == workId)
                    .ToList()
                    .OrderByDescending(x => x.KeyOrder)
                    .ToList();
            }

            if (workId == 0 && clientId != 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.InvoiceToCancel)
                    .Include(x => x.Work)
                    .ThenInclude(x => x.WorkBudgets)
                    .Where(x => x.Work.ClientId == clientId)
                    .ToList()
                    .OrderByDescending(x => x.KeyOrder)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Work.Name?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Work.Client.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.TaxBase.ToString().ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization((Convert.ToDouble(x.TaxBase) + (Convert.ToDouble(x.TaxBase) * 0.21)).ToString()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.StartDate.ToString("dd/MM/yyyyy")).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.EndDate.ToString("dd/MM/yyyyy")).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.IssueDate.ToString("dd/MM/yyyyy")).Contains(filter))
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
            var invoice = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Invoices)
                    .Include(x => x.InvoiceToCancel)
                    .ThenInclude(x => x.DetailsInvoice)
                    .Include(x => x.InvoiceToCancel)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.Work)
                    .ThenInclude(x => x.WorkBudgets)
                    .Include(x => x.DetailsInvoice)
                    .Include(x => x.WorkBudget)
                .FirstOrDefault(x => x.Id == id);

            if (invoice == null)
                return null;

            invoice.IvaTaxBase = invoice.TaxBase * invoice.Work.PercentageIVA;
            invoice.Total = invoice.IvaTaxBase + (double)invoice.TaxBase;

            return invoice;
        }

        public Invoice Add(Invoice newInvoice)
        {
            Validate(newInvoice);

            var invoiceNumber = CountInvoicesInYear(newInvoice.IssueDate.Year);
            var work = _context.Work.FirstOrDefault(x => x.Id == newInvoice.WorkId);

            newInvoice.InvoiceNumber = invoiceNumber;
            newInvoice.Name = work != null
                ? $"{work.WorksToRealize}{invoiceNumber:0000}_{newInvoice.IssueDate.Year.ToString().Substring(2, 2)}"
                : $"{invoiceNumber:0000}_{newInvoice.IssueDate.Year.ToString().Substring(2, 2)}";

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
                        Work work = null;
                        if (invoice.WorkId != null)
                        {
                            work = _context.Work.FirstOrDefault(x => x.Id == invoice.WorkId);
                            invoice.Iva = work != null ? !work.PassiveSubject : true;
                        }
                        else
                        {
                            invoice.Iva = true;
                        }

                        invoice.IvaTaxBase = invoice.Iva == true ? Math.Round((double)invoice.TaxBase * 0.21, 2) : 0;
                        invoice.Total = invoice.IvaTaxBase + (double)invoice.TaxBase;

                        var invoiceNumber = CountInvoicesInYear(invoice.IssueDate.Year);

                        invoice.InvoiceNumber = invoiceNumber;
                        invoice.Name = work != null
                            ? $"{work.WorksToRealize}{invoiceNumber:0000}_{invoice.IssueDate.Year.ToString().Substring(2, 2)}"
                            : $"{invoiceNumber:0000}_{invoice.IssueDate.Year.ToString().Substring(2, 2)}";

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

        public int? CheckInvoice(Invoice newInvoice)
        {
            var invoice = _context.Invoice
                                    .FirstOrDefault(x =>
                                        x.WorkId == newInvoice.WorkId &&
                                        x.StartDate == newInvoice.StartDate &&
                                        x.EndDate == newInvoice.EndDate);
            if (invoice == null)
                return null;

            if (newInvoice.TaxBase == invoice.TaxBase)
            {
                return 0;
            }

            return invoice.Id;
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

            if (_context.Invoice.FirstOrDefault(x => x.InvoiceToCancelId == id) != null)
                throw new Exception("No se puede eliminar una Factura que está anulada");

            var toRemove = _context.Invoice.Find(id);
            _context.Invoice.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }

        public int CountInvoices()
        {
            return _context.Invoice.Count();
        }

        public int CountInvoicesInYear(int year)
        {
            var invoiceNumber = 0;
            var invoices = _context.Invoice
                .Where(x => x.IssueDate >= new DateTime(year, 1, 1) && x.IssueDate <= new DateTime(year, 12, 31));
            if (invoices.Count() > 0)
            {
                invoiceNumber = invoices.Select(x => x.InvoiceNumber).Max();
            }
            invoiceNumber++;

            return invoiceNumber;
        }

        #region Auxiliary Methods

        private void Validate(Invoice invoice)
        {
            if (invoice.ClientId == null ||
                invoice.WorkId == null)
            {
                throw new Exception("Factura incompleta. Revisa los datos");
            }
        }

        #endregion
    }
}
