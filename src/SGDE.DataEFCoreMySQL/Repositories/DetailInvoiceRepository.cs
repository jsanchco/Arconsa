namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class DetailInvoiceRepository : IDetailInvoiceRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public DetailInvoiceRepository(EFContextMySQL context)
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
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        private bool DetailInvoiceExists(int id)
        {
            return GetById(id) != null;
        }

        public List<DetailInvoice> GetAll(int invoiceId = 0, bool previousInvoice = false)
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
            _context.DetailInvoice.Add(newDetailInvoice);
            _context.SaveChanges();
            return newDetailInvoice;
        }

        public bool Update(DetailInvoice detailInvoice)
        {
            if (!DetailInvoiceExists(detailInvoice.Id))
                return false;

            _context.DetailInvoice.Update(detailInvoice);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!DetailInvoiceExists(id))
                return false;

            var toRemove = _context.DetailInvoice.Find(id);
            _context.DetailInvoice.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
