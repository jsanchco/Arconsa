using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreMySQL.Repositories
{
    public class AdvanceRepository : IAdvanceRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public AdvanceRepository(EFContextMySQL context)
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

        private bool AdvanceExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Advance> GetAll(int skip = 0, int take = 0, int userId = 0)
        {
            List<Advance> data;

            if (userId != 0)
            {
                data = _context.Advance
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .OrderBy(x => x.ConcessionDate)
                    .ToList();
            }
            else
            {
                data = _context.Advance
                    .Include(x => x.User)
                    .OrderBy(x => x.ConcessionDate)
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<Advance>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<Advance>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public Advance GetById(int id)
        {
            return _context.Advance
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public Advance Add(Advance newAdvance)
        {
            _context.Advance.Add(newAdvance);
            _context.SaveChanges();
            return newAdvance;
        }

        public bool Update(Advance advance)
        {
            if (!AdvanceExists(advance.Id))
                return false;

            _context.Advance.Update(advance);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!AdvanceExists(id))
                return false;

            var toRemove = _context.Advance.Find(id);
            _context.Advance.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}

