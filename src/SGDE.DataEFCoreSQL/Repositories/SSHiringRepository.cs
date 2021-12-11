using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class SSHiringRepository : ISSHiringRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public SSHiringRepository(EFContextSQL context)
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

        private bool SSHiringExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<SSHiring> GetAll(int skip = 0, int take = 0, int userId = 0)
        {
            List<SSHiring> data = new List<SSHiring>();

            if (userId == 0)
            {
                data = _context.SSHiring
                    .Include(x => x.User)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (userId != 0)
            {
                data = _context.SSHiring
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<SSHiring>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<SSHiring>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public SSHiring GetById(int id)
        {
            return _context.SSHiring
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public SSHiring Add(SSHiring newSSHiring)
        {
            _context.SSHiring.Add(newSSHiring);
            _context.SaveChanges();
            return newSSHiring;
        }

        public bool Update(SSHiring sSHiring)
        {
            if (!SSHiringExists(sSHiring.Id))
                return false;

            _context.SSHiring.Update(sSHiring);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!SSHiringExists(id))
                return false;

            var toRemove = _context.SSHiring.Find(id);
            _context.SSHiring.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
