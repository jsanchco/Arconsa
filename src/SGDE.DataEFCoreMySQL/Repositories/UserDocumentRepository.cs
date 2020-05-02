namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class UserDocumentRepository : IUserDocumentRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public UserDocumentRepository(EFContextMySQL context)
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
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        private bool UserDocumentExists(int id)
        {
            return GetById(id) != null;
        }

        public List<UserDocument> GetAll(int userId)
        {
            if (userId != 0)
            {
                return _context.UserDocument
                    .Include(x => x.TypeDocument)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }

            return _context.UserDocument
                .Include(x => x.TypeDocument)
                .Include(x => x.User)
                .ToList();
        }

        public UserDocument GetById(int id)
        {
            return _context.UserDocument
                .Include(x => x.TypeDocument)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public UserDocument Add(UserDocument newUserDocument)
        {
            _context.UserDocument.Add(newUserDocument);
            _context.SaveChanges();
            return newUserDocument;
        }

        public bool Update(UserDocument userDocument)
        {
            if (!UserDocumentExists(userDocument.Id))
                return false;

            _context.UserDocument.Update(userDocument);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!UserDocumentExists(id))
                return false;

            var toRemove = _context.UserDocument.Find(id);
            _context.UserDocument.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
