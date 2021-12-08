using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreMySQL.Repositories
{
    public class DetailEmbargoRepository : IDetailEmbargoRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public DetailEmbargoRepository(EFContextMySQL context)
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

        private bool DetailEmbargoExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<DetailEmbargo> GetAll(int skip = 0, int take = 0, int embargoId = 0)
        {
            List<DetailEmbargo> data = new List<DetailEmbargo>();

            if (embargoId == 0)
            {
                data = _context.DetailEmbargo
                    .Include(x => x.Embargo)
                    .OrderByDescending(x => x.DatePay)
                    .ToList();
            }

            if (embargoId != 0)
            {
                data = _context.DetailEmbargo
                    .Include(x => x.Embargo)
                    .Where(x => x.EmbargoId == embargoId)
                    .OrderByDescending(x => x.DatePay)
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<DetailEmbargo>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<DetailEmbargo>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public DetailEmbargo GetById(int id)
        {
            return _context.DetailEmbargo
                .Include(x => x.Embargo)
                .FirstOrDefault(x => x.Id == id);
        }

        public DetailEmbargo Add(DetailEmbargo newDetailEmbargo, bool isPaid)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.DetailEmbargo.Add(newDetailEmbargo);
                    if (isPaid)
                    {
                        var embargo = _context.Embargo.Find(newDetailEmbargo.EmbargoId);
                        embargo.Paid = isPaid;
                        _context.Embargo.Update(embargo);
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return newDetailEmbargo;
        }

        public bool Update(DetailEmbargo detailEmbargo, bool isPaid)
        {
            if (!DetailEmbargoExists(detailEmbargo.Id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.DetailEmbargo.Update(detailEmbargo);

                    var embargo = _context.Embargo.Find(detailEmbargo.EmbargoId);
                    embargo.Paid = isPaid;
                    _context.Embargo.Update(embargo);
                    _context.SaveChanges();

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
            if (!DetailEmbargoExists(id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var toRemove = _context.DetailEmbargo.Find(id);
                    _context.DetailEmbargo.Remove(toRemove);

                    var embargo = _context.Embargo.Find(toRemove.EmbargoId);
                    embargo.Paid = false;
                    _context.Embargo.Update(embargo);
                    _context.SaveChanges();

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

