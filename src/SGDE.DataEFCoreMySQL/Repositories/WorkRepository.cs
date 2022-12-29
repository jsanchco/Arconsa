namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class WorkRepository : IWorkRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public WorkRepository(EFContextMySQL context)
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

        private bool WorkExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Work> GetAll(int skip = 0, int take = 0, string filter = null, int clientId = 0, bool showCloseWorks = true)
        {
            List<Work> data;

            if (clientId == 0)
            {
                data = _context.Work
                        .Include(x => x.Client)
                        .Include(x => x.UserHirings)
                        //.Include(x => x.WorkBudgets)
                        .Include(x => x.WorkStatusHistories)
                        .ToList();
            }
            else
            {
                data = _context.Work
                        .Include(x => x.Client)
                        .Include(x => x.UserHirings)
                        //.Include(x => x.WorkBudgets)
                        .Where(x => x.ClientId == clientId)
                        .Include(x => x.WorkStatusHistories)
                        .ToList();
            }

            if (!showCloseWorks)
            {
                data = data
                    .Where(x => x.CloseDate == null)
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

        public List<Work> GetAllLiteIncludeClient(string filter = null)
        {
            var result = _context.Work
                    .Include(x => x.Client)
                    .ToList();

            if (!string.IsNullOrEmpty(filter))
            {
                result = result
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Client?.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            return result;
        }

        public List<Work> GetAllWorkBetweenDates(DateTime startDate, DateTime endDate)
        {
            List<Work> data = _context.Work
                        .Include(x => x.Client)
                        .Include(x => x.Invoices)
                        .Include(x => x.WorkBudgets)
                        .Include(x => x.WorkStatusHistories)
                        .Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate)
                        .ToList();

            return data;
        }

        public Work GetById(int id)
        {
            return _context.Work
                .Include(x => x.Client)                
                .ThenInclude(x => x.ProfessionInClients)
                .Include(x => x.UserHirings)
                .Include(x => x.Invoices)
                .Include(x => x.WorkBudgets)
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
                            if (userHiring.EndDate == null)
                            {
                                userHiring.EndDate = DateTime.Now;
                                _context.UserHiring.Update(userHiring);
                                _context.SaveChanges();
                            }
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

        public List<Work> GetAllLite(string filter = null, int clientId = 0)
        {
            List<Work> data;

            if (clientId != 0)
            {
                data = _context.Work
                    .Where(x => x.ClientId == clientId)
                    .Select(x => new Work
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }
            else
            {
                data = _context.Work
                    .Select(x => new Work
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }

            if (filter != null)
            {
                data = data.Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            return data;
        }

        public int Count()
        {
            return _context.Work.Count();
        }
    }
}
