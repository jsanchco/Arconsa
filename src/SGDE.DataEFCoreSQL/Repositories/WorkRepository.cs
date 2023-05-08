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

    public class WorkRepository : IWorkRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public WorkRepository(EFContextSQL context)
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

        private bool WorkExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Work> GetAll(int skip = 0, int take = 0, int enterpriseId = 0, string filter = null, int clientId = 0, bool showCloseWorks = true)
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
                        .Include(x => x.WorkStatusHistories)
                        .Where(x => x.ClientId == clientId)
                        .ToList();
            }

            if (enterpriseId != 0)
            {
                data = data
                    .Where(x => x.Client.EnterpriseId == enterpriseId)
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
                        Searcher.RemoveAccentsWithNormalization(x.Client?.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Status?.ToLower()).Contains(filter))
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

        public List<Work> GetAllLiteIncludeClient(int enterpriseId = 0, string filter = null)
        {
            var result = _context.Work
                    .Include(x => x.Client)
                    .Where(x => x.Client.EnterpriseId == enterpriseId)
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

        public List<Work> GetAllWorkBetweenDates(int enterpriseId, DateTime startDate, DateTime endDate)
        {
            List<Work> data = _context.Work
                        .Include(x => x.Client)
                        .Include(x => x.Invoices)
                        .Include(x => x.WorkBudgets)
                        .Include(x => x.WorkStatusHistories)
                        .Where(x => x.Client.EnterpriseId == enterpriseId && x.OpenDate >= startDate && x.OpenDate <= endDate)
                        .ToList();

            return data;
        }

        public Work GetById(int id)
        {
            var result = _context.Work
                .Include(x => x.Client)
                .ThenInclude(x => x.ProfessionInClients)
                .Include(x => x.UserHirings)
                .Include(x => x.Invoices)
                .Include(x => x.WorkBudgets)
                .Include(x => x.WorkStatusHistories)
                //.Include(x => x.WorkCosts)
                .FirstOrDefault(x => x.Id == id);

            return result;
        }

        public Work Add(Work newWork)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Work.Add(newWork);
                    _context.SaveChanges();

                    _context.WorkStatusHistory.Add(new WorkStatusHistory
                    {
                        WorkId = newWork.Id,
                        AddedDate = DateTime.Now,
                        Observations = "Apertura de Obra",
                        Value = "Abierta",
                        DateChange = DateTime.Now
                    });
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

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
                        workOld.Open == false && work.Open == false)
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

        public List<Work> GetAllLite(int enterpriseId = 0, string filter = null, int clientId = 0)
        {
            List<Work> data;

            if (clientId != 0)
            {
                data = _context.Work
                    .Include(x => x.Client)
                    .Where(x => x.Client.EnterpriseId == enterpriseId && x.ClientId == clientId)
                    .Select(x => new Work
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }
            else
            {
                data = _context.Work
                    .Include(x => x.Client)
                    .Where(x => x.Client.EnterpriseId == enterpriseId && x.ClientId == clientId)
                    .Select(x => new Work
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }

            if (filter != null)
            {
                data = int.TryParse(filter, out int number)
                    ? data
                        .Where(x => x.Id == number ||
                                    x.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
                        .ToList()
                    : data
                        .Where(x => x.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
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
