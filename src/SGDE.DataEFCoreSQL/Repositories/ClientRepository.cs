namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using SGDE.Domain.Helpers;
    using System;

    #endregion

    public class ClientRepository : IClientRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        private int test => string.IsNullOrEmpty("") ? 0 : 1;
        public ClientRepository(EFContextSQL context)
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

        private bool ClientExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Client> GetAllWithoutFilter()
        {
            return _context.Client
                .Include(x => x.ClientResponsibles)
                .Include(x => x.ProfessionInClients)
                .ToList();
        }

        public QueryResult<Client> GetAll(int skip = 0, int take = 0, string filter = null)
        {
            var data = _context.Client
                .Include(x => x.ClientResponsibles)
                .Include(x => x.ProfessionInClients)
                .ToList();           

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Cif?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.WayToPay?.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<Client>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<Client>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
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

        public List<Client> GetAllLite(string filter = null)
        {
            var data = _context.Client
                .Select(x => new Client
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            if (filter != null)
            {
                data = data
                    .Where(x => x.Id == Convert.ToInt32(filter))
                    .ToList();
            }

            return data;
        }
    }
}
