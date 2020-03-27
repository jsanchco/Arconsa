namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class ClientRepository : IClientRepository
    {
        private readonly EFContextSQL _context;

        public ClientRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool ClientExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Client> GetAll()
        {
            return _context.Client
                .Include(x => x.ClientResponsibles)
                .Include(x => x.ProfessionInClients)
                .ToList();
        }

        public Client GetById(int id)
        {
            return _context.Client
                .Include(x => x.ClientResponsibles)
                .Include(x => x.ProfessionInClients)
                .FirstOrDefault(x => x.Id == id);
        }

        public Client Add(Client newClient)
        {
            _context.Client.Add(newClient);
            _context.SaveChanges();
            return newClient;
        }

        public bool Update(Client client)
        {
            if (!ClientExists(client.Id))
                return false;

            _context.Client.Update(client);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!ClientExists(id))
                return false;

            var toRemove = _context.Client.Find(id);
            _context.Client.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
