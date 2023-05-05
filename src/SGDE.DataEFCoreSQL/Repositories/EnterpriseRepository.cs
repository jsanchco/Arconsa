using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class EnterpriseRepository : IEnterpriseRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public EnterpriseRepository(EFContextSQL context)
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

        private bool EnterpriseExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Enterprise> GetAll()
        {
            return _context.Enterprise
                .Include(x => x.UsersEnterprises)
                .ToList();
        }

        public List<Enterprise> GetByUserId(int userId)
        {
            return _context.UserEnterprise
                .Include(x => x.User)
                .Include(x => x.Enterprise)
                .Where(x => x.UserId == userId)
                .Select(x => x.Enterprise)
                .ToList();
        }

        public Enterprise GetById(int id)
        {
            return _context.Enterprise
                .Include(x => x.UsersEnterprises)
                .FirstOrDefault(x => x.Id == id);
        }

        public Enterprise Add(Enterprise newEnterprise)
        {
            _context.Enterprise.Add(newEnterprise);
            _context.SaveChanges();
            return newEnterprise;
        }

        public bool Update(Enterprise enterprise)
        {
            if (!EnterpriseExists(enterprise.Id))
                return false;

            _context.Enterprise.Update(enterprise);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!EnterpriseExists(id))
                return false;

            var toRemove = _context.Enterprise.Find(id);
            _context.Enterprise.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
