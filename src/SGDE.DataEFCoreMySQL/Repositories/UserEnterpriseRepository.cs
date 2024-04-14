using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;

namespace SGDE.DataEFCoreMySQL.Repositories
{
    public class UserEnterpriseRepository : IUserEnterpriseRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public UserEnterpriseRepository(EFContextMySQL context)
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
        public UserEnterprise Add(UserEnterprise newUserEnterprise)
        {
            _context.UserEnterprise.Add(newUserEnterprise);
            _context.SaveChanges();
            return newUserEnterprise;
        }
    }
}
