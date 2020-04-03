namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

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
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (workId != 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Where(x => x.WorkId == workId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (workId == 0 && clientId != 0)
            {
                data = _context.Invoice
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
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
                .FirstOrDefault(x => x.Id == id);
        }

        public Invoice Add(Invoice newInvoice)
        {
            _context.Invoice.Add(newInvoice);
            _context.SaveChanges();
            return newInvoice;
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
    }
}
