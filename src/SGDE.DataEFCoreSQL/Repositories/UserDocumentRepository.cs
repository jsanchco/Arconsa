namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class UserDocumentRepository : IUserDocumentRepository
    {
        private readonly EFContextSQL _context;

        public UserDocumentRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool UserDocumentExists(int id)
        {
            return GetById(id) != null;
        }

        public List<UserDocument> GetAll()
        {
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
