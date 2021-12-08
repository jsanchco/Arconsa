using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class EmbargoRepository : IEmbargoRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public EmbargoRepository(EFContextSQL context)
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

        private bool EmbargoExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Embargo> GetAll(int skip = 0, int take = 0, int userId = 0)
        {
            List<Embargo> data = new List<Embargo>();

            if (userId == 0)
            {
                data = _context.Embargo
                    .Include(x => x.User)
                    .Include(x => x.DetailEmbargos)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (userId != 0)
            {
                data = _context.Embargo
                    .Include(x => x.User)
                    .Include(x => x.DetailEmbargos)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<Embargo>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<Embargo>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };

        }

        public Embargo GetById(int id)
        {
            return _context.Embargo
                .Include(x => x.User)
                .Include(x => x.DetailEmbargos)
                .FirstOrDefault(x => x.Id == id);
        }

        public Embargo Add(Embargo newEmbargo)
        {
            _context.Embargo.Add(newEmbargo);
            _context.SaveChanges();
            return newEmbargo;
        }

        public bool Update(Embargo embargo)
        {
            if (!EmbargoExists(embargo.Id))
                return false;

            _context.Embargo.Update(embargo);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!EmbargoExists(id))
                return false;

            var toRemove = _context.Embargo.Find(id);
            _context.Embargo.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
