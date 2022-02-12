﻿namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using System;
    using Microsoft.EntityFrameworkCore;

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

        public List<DetailInvoice> UpdateFromPreviousInvoice(int invoiceId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = _context.Invoice
                        .Include(x => x.Work)
                        .FirstOrDefault(x => x.Id == invoiceId);
                    if (invoice == null)
                        throw new Exception("Factura no encontrada");
                    if (!invoice.Work.InvoiceToOrigin)
                        throw new Exception("Esta Factura no es a Origen");

                    var detailsInvoice = _context.DetailInvoice.Where(x => x.InvoiceId == invoiceId);
                    _context.DetailInvoice.RemoveRange(detailsInvoice);

                    var invoiceFind = _context.Invoice
                        .Include(x => x.DetailsInvoice)
                        .Where(x => x.WorkId == invoice.WorkId && x.EndDate < invoice.StartDate)
                        .OrderByDescending(x => x.StartDate)
                        .FirstOrDefault();
                    if (invoiceFind == null)
                        throw new Exception("Factura no encontrada");

                    foreach (var detailInvoice in invoiceFind.DetailsInvoice)
                    {
                        detailInvoice.Id = 0;
                        detailInvoice.InvoiceId = invoiceId;
                        detailInvoice.UnitsAccumulated = detailInvoice.Units;
                        detailInvoice.Units = 0;

                        _context.DetailInvoice.Add(detailInvoice);
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return _context.DetailInvoice
                .Where(x => x.InvoiceId == invoiceId)
                .ToList();
        }
    }
}
