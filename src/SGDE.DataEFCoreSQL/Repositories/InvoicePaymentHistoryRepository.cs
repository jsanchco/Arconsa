using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class InvoicePaymentHistoryRepository : IInvoicePaymentHistoryRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public InvoicePaymentHistoryRepository(EFContextSQL context)
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

        private bool InvoicePaymentHistoryExists(int id)
        {
            return GetById(id) != null;
        }

        public List<InvoicePaymentHistory> GetAll(int invoiceId)
        {
            List<InvoicePaymentHistory> data;

            if (invoiceId == 0)
            {
                data = _context.InvoicePaymentHistory
                        .ToList();
            }
            else
            {
                data = _context.InvoicePaymentHistory
                        .Where(x => x.InvoiceId == invoiceId)
                        .ToList();
            }

            return data;
        }

        public InvoicePaymentHistory GetById(int id)
        {
            var result = _context.InvoicePaymentHistory
                .Include(x => x.Invoice)
                .FirstOrDefault(x => x.Id == id);

            return result;
        }

        public InvoicePaymentHistory Add(InvoicePaymentHistory newInvoicePaymentHistory)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = _context.Invoice
                        .FirstOrDefault(x => x.Id == newInvoicePaymentHistory.InvoiceId);
                    if (invoice == null)
                        throw new Exception($"Invoice [{newInvoicePaymentHistory.InvoiceId}] NOT Found");

                    _context.InvoicePaymentHistory.Add(newInvoicePaymentHistory);
                    _context.SaveChanges();

                    var total = _context.InvoicePaymentHistory.Sum(x => x.Amount);

                    invoice.ModifiedDate = DateTime.Now;
                    invoice.TotalPayment = total;
                    invoice.PayDate = newInvoicePaymentHistory.DatePayment;

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return newInvoicePaymentHistory;
        }

        public bool Update(InvoicePaymentHistory invoicePaymentHistory)
        {
            if (!InvoicePaymentHistoryExists(invoicePaymentHistory.Id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = _context.Invoice
                        .FirstOrDefault(x => x.Id == invoicePaymentHistory.InvoiceId);
                    if (invoice == null)
                        throw new Exception($"Invoice [{invoicePaymentHistory.InvoiceId}] NOT Found");

                    _context.InvoicePaymentHistory.Update(invoicePaymentHistory);
                    _context.SaveChanges();

                    var total = _context.InvoicePaymentHistory.Sum(x => x.Amount);

                    invoice.ModifiedDate = DateTime.Now;
                    invoice.TotalPayment = total;

                    if (_context.InvoicePaymentHistory.Count() == 0)
                    {
                        invoice.PayDate = null;
                    }
                    else
                    {
                        var maxDate = _context.InvoicePaymentHistory.Max(x => x.DatePayment);
                        invoice.PayDate = maxDate;
                    }

                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public bool Delete(int id)
        {
            if (!InvoicePaymentHistoryExists(id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var toRemove = _context.InvoicePaymentHistory.Find(id);
                    _context.InvoicePaymentHistory.Remove(toRemove);
                    _context.SaveChanges();

                    var invoice = _context.Invoice
                        .FirstOrDefault(x => x.Id == toRemove.InvoiceId);
                    if (invoice == null)
                        throw new Exception($"Invoice [{id}] NOT Found");

                    var total = _context.InvoicePaymentHistory.Sum(x => x.Amount);

                    invoice.ModifiedDate = DateTime.Now;
                    invoice.TotalPayment = total;

                    if (_context.InvoicePaymentHistory.Count() == 0)
                    {
                        invoice.PayDate = null;
                    }
                    else
                    {
                        var maxDate = _context.InvoicePaymentHistory.Max(x => x.DatePayment);
                        invoice.PayDate = maxDate;
                    }

                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
