namespace SGDE.DataEFCoreMySQL.Repositories
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
        private readonly EFContextMySQL _context;

        public UserHiringRepository(EFContextMySQL context)
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

        public List<UserHiring> GetAll(int userId = 0, int workId = 0)
        {
            if (userId == 0 && workId == 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .ToList();
            }

            if (userId != 0 && workId == 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }

            if (userId == 0 && workId != 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Where(x => x.WorkId == workId)
                    .ToList();
            }

            if (userId != 0 && workId != 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId && x.WorkId == workId)
                    .ToList();
            }

            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .ToList();
        }

        public List<UserHiring> GetOpen()
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .Where(x => x.EndDate == null)
                .ToList();
        }

        public UserHiring GetById(int id)
        {
            return _context.UserHiring
                .Include(x => x.Work)
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

            var toRemove = _context.Role.Find(id);
            _context.Role.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
