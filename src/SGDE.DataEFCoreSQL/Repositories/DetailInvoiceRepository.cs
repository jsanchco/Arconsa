namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using System;

    #endregion

    public class DetailInvoiceRepository : IDetailInvoiceRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public DetailInvoiceRepository(EFContextSQL context)
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

        private bool DetailInvoiceExists(int id)
        {
            return GetById(id) != null;
        }

        public List<DetailInvoice> GetAll(int invoiceId = 0)
        {
            if (invoiceId == 0)
            {
                return _context.DetailInvoice
                    .ToList();
            }

            return _context.DetailInvoice
                .Where(x => x.InvoiceId == invoiceId)
                .ToList();
        }

        public DetailInvoice GetById(int id)
        {
            return _context.DetailInvoice
                .FirstOrDefault(x => x.Id == id);
        }

        public DetailInvoice Add(DetailInvoice newDetailInvoice)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = _context.Invoice.FirstOrDefault(x => x.Id == newDetailInvoice.InvoiceId);
                    if (invoice == null)
                        throw new Exception("Factura no encontrada");

                    _context.DetailInvoice.Add(newDetailInvoice);

                    invoice.TaxBase += newDetailInvoice.Units * newDetailInvoice.PriceUnity;
                    _context.Invoice.Update(invoice);

                    _context.SaveChanges();
                    transaction.Commit();

                    return newDetailInvoice;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public bool Update(DetailInvoice detailInvoice)
        {
            if (!DetailInvoiceExists(detailInvoice.Id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = _context.Invoice.FirstOrDefault(x => x.Id == detailInvoice.InvoiceId);
                    if (invoice == null)
                        throw new Exception("Factura no encontrada");

                    _context.DetailInvoice.Update(detailInvoice);
                    _context.SaveChanges();

                    var total =_context.DetailInvoice
                        .Where(x => x.InvoiceId == detailInvoice.InvoiceId)
                        .ToList()
                        .Sum(x => x.Total);

                    invoice.TaxBase = (decimal)total;
                    _context.Invoice.Update(invoice);
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
            if (!DetailInvoiceExists(id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var toRemove = _context.DetailInvoice.Find(id);
                    var invoice = _context.Invoice.FirstOrDefault(x => x.Id == toRemove.InvoiceId);
                    if (invoice == null)
                        throw new Exception("Factura no encontrada");

                    _context.DetailInvoice.Remove(toRemove);
                    _context.SaveChanges();

                    var total = _context.DetailInvoice
                        .Where(x => x.InvoiceId == toRemove.InvoiceId)
                        .ToList()
                        .Sum(x => x.Total);

                    invoice.TaxBase = (decimal)total;
                    _context.Invoice.Update(invoice);
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
