namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class UserHiringRepository : IUserHiringRepository
    {
        private readonly EFContextSQL _context;

        public UserHiringRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool UserHiringExists(int id)
        {
            return GetById(id) != null;
        }

        public List<UserHiring> GetAll()
        {
            return _context.UserHiring
                .Include(x => x.Builder)
                .Include(x => x.User)
                .ToList();
        }

        public UserHiring GetById(int id)
        {
            return _context.UserHiring
                .Include(x => x.Builder)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public UserHiring Add(UserHiring newUserHiring)
        {
            _context.UserHiring.Add(newUserHiring);
            _context.SaveChanges();
            return newUserHiring;
        }

        public bool Update(UserHiring userHiring)
        {
            if (!UserHiringExists(userHiring.Id))
                return false;

            _context.UserHiring.Update(userHiring);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!UserHiringExists(id))
                return false;

            var toRemove = _context.UserHiring.Find(id);
            _context.UserHiring.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
