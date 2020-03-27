namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using SGDE.Domain.Helpers;

    #endregion

    public class ProfessionInClientRepository: IProfessionInClientRepository
    {
        private readonly EFContextSQL _context;

        public ProfessionInClientRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool ProfessionInClientExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<ProfessionInClient> GetAll(int skip = 0, int take = 0, string filter = null, int professionId = 0, int clientId = 0)
        {
            var data = new List<ProfessionInClient>();

            if (clientId == 0 && professionId == 0)
            {
                data = _context.ProfessionInClient
                        .Include(x => x.Profession)
                        .Include(x => x.Client)
                        .ToList();
            }
            if (clientId != 0 && professionId == 0)
            {
                data = _context.ProfessionInClient
                        .Include(x => x.Profession)
                        .Include(x => x.Client)
                        .Where(x => x.ClientId == clientId)
                        .ToList();
            }

            if (clientId == 0 && professionId != 0)
            {
                data = _context.ProfessionInClient
                        .Include(x => x.Profession)
                        .Include(x => x.Client)
                        .Where(x => x.ProfessionId == professionId)
                        .ToList();
            }

            if (clientId != 0 && professionId != 0)
            {
                data = _context.ProfessionInClient
                        .Include(x => x.Profession)
                        .Include(x => x.Client)
                        .Where(x => x.ClientId == clientId && x.ProfessionId == professionId)
                        .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Profession.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Client.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<ProfessionInClient>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<ProfessionInClient>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public ProfessionInClient GetById(int id)
        {
            return _context.ProfessionInClient
                .Include(x => x.Client)
                .Include(x => x.Profession)
                .FirstOrDefault(x => x.Id == id);
        }

        public ProfessionInClient Add(ProfessionInClient newProfessionInClient)
        {
            _context.ProfessionInClient.Add(newProfessionInClient);
            _context.SaveChanges();
            return newProfessionInClient;
        }

        public bool Update(ProfessionInClient professionInClient)
        {
            if (!ProfessionInClientExists(professionInClient.Id))
                return false;

            _context.ProfessionInClient.Update(professionInClient);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!ProfessionInClientExists(id))
                return false;

            var toRemove = _context.ProfessionInClient.Find(id);
            _context.ProfessionInClient.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
