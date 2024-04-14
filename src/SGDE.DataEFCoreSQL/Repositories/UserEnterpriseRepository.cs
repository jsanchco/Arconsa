using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class UserEnterpriseRepository : IUserEnterpriseRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public UserEnterpriseRepository(EFContextSQL context)
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
