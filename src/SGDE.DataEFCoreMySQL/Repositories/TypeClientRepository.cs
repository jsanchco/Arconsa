namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;

    #endregion

    public class TypeClientRepository : ITypeClientRepository
    {
        private readonly EFContextMySQL _context;

        public TypeClientRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool TypeClientExists(int id)
        {
            return GetById(id) != null;
        }

        public List<TypeClient> GetAll()
        {
            return _context.TypeClient
                .ToList();
        }

        public TypeClient GetById(int id)
        {
            return _context.TypeClient
                .FirstOrDefault(x => x.Id == id);
        }

        public TypeClient Add(TypeClient newTypeClient)
        {
            _context.TypeClient.Add(newTypeClient);
            _context.SaveChanges();
            return newTypeClient;
        }

        public bool Update(TypeClient typeClient)
        {
            if (!TypeClientExists(typeClient.Id))
                return false;

            _context.TypeClient.Update(typeClient);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!TypeClientExists(id))
                return false;

            var toRemove = _context.TypeClient.Find(id);
            _context.TypeClient.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
