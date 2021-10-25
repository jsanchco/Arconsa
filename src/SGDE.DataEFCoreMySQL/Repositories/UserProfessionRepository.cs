using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreMySQL.Repositories
{
    public class UserProfessionRepository : IUserProfessionRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public UserProfessionRepository(EFContextMySQL context)
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

        private bool UserProfessionExists(int userId, int professionId)
        {
            return _context.UserProfession
                .FirstOrDefault(x => x.UserId == userId && x.ProfessionId == professionId) != null;
        }

        public UserProfession Add(UserProfession newUserProfession)
        {
            _context.UserProfession.Add(newUserProfession);
            _context.SaveChanges();

            return newUserProfession;
        }

        public bool Delete(int userProfessionId)
        {
            var findUserProfession = _context.UserProfession.FirstOrDefault(x => x.Id == userProfessionId);
            if (findUserProfession == null)
                return false;

            var toRemove = _context.UserProfession.Find(findUserProfession.Id);
            _context.UserProfession.Remove(toRemove);
            _context.SaveChanges();

            return true;
        }

        public bool Delete(int userId, int professionId)
        {
            var findUserProfession = _context.UserProfession
                .FirstOrDefault(x => x.UserId == userId && x.ProfessionId == professionId);
            if (findUserProfession == null)
                return false;

            var toRemove = _context.UserProfession.Find(findUserProfession.Id);
            _context.UserProfession.Remove(toRemove);
            _context.SaveChanges();

            return true;
        }

        public List<UserProfession> GetAll(int userId)
        {
            return _context.UserProfession
                .Include(x => x.User)
                .Include(x => x.Profession)
                .Where(x => x.UserId == userId)
                .ToList();
        }

        public UserProfession GetById(int id)
        {
            return _context.UserProfession
                .Include(x => x.User)
                .Include(x => x.Profession)
                .FirstOrDefault(x => x.Id == id);
        }

        public bool Update(UserProfession userProfession)
        {
            if (!UserProfessionExists(userProfession.Id, userProfession.ProfessionId))
                return false;

            _context.UserProfession.Update(userProfession);
            _context.SaveChanges();

            return true;
        }
    }
}
