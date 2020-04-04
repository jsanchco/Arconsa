namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;

    #endregion

    public class DetailInvoiceRepository : IDetailInvoiceRepository
    {
        private readonly EFContextSQL _context;

        public DetailInvoiceRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool DetailInvoiceExists(int id)
        {
            return GetById(id) != null;
        }

        public List<DetailInvoice> GetAll()
        {
            return _context.DetailInvoice
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
