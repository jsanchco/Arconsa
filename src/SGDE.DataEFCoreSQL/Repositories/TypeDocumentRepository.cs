namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;

    #endregion

    public class TypeDocumentRepository : ITypeDocumentRepository
    {
        private readonly EFContextSQL _context;

        public TypeDocumentRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool TypeDocumentExists(int id)
        {
            return GetById(id) != null;
        }

        public List<TypeDocument> GetAll()
        {
            return _context.TypeDocument
                .ToList();
        }

        public TypeDocument GetById(int id)
        {
            return _context.TypeDocument
                .FirstOrDefault(x => x.Id == id);
        }

        public TypeDocument Add(TypeDocument newTypeDocument)
        {
            _context.TypeDocument.Add(newTypeDocument);
            _context.SaveChanges();
            return newTypeDocument;
        }

        public bool Update(TypeDocument typeDocument)
        {
            if (!TypeDocumentExists(typeDocument.Id))
                return false;

            _context.TypeDocument.Update(typeDocument);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!TypeDocumentExists(id))
                return false;

            var toRemove = _context.TypeDocument.Find(id);
            _context.TypeDocument.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
