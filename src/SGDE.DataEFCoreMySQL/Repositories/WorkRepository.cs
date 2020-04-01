namespace SGDE.DataEFCoreMySQL.Repositories
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

    public class WorkRepository : IWorkRepository
    {
        private readonly EFContextMySQL _context;

        public WorkRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool WorkExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Work> GetAll(int skip = 0, int take = 0, string filter = null, int clientId = 0)
        {
            List<Work> data;

            if (clientId == 0)
            {
                data = _context.Work
                        .Include(x => x.Client)
                        .ToList();
            }
            else
            {
                data = _context.Work
                        .Include(x => x.Client)
                        .Where(x => x.ClientId == clientId)
                        .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Client?.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<Work>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<Work>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public Work GetById(int id)
        {
            return _context.Work
                .Include(x => x.Client)
                .ThenInclude(x => x.ProfessionInClients)
                .FirstOrDefault(x => x.Id == id);
        }

        public Work Add(Work newWork)
        {
            _context.Work.Add(newWork);
            _context.SaveChanges();
            return newWork;
        }

        public bool Update(Work work)
        {
            if (!WorkExists(work.Id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var workOld = GetById(work.Id);
                    if (workOld != null &&
                        workOld.Open == true && work.Open == false)
                    {
                        foreach (var userHiring in workOld.UserHirings)
                        {
                            userHiring.EndDate = DateTime.Now;
                            _context.UserHiring.Update(userHiring);
                            _context.SaveChanges();
                        }
                    }

                    _context.Work.Update(work);
                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public bool Delete(int id)
        {
            if (!WorkExists(id))
                return false;

            var toRemove = _context.Work.Find(id);
            _context.Work.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
