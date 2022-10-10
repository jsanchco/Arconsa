using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class WorkHistoryRepository : IWorkHistoryRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public WorkHistoryRepository(EFContextSQL context)
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

        private bool WorkHistoryExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<WorkHistory> GetAll(int workId, int skip = 0, int take = 0, string filter = null)
        {
            List<WorkHistory> data;

            if (workId != 0)
            {
                data = _context.WorkHistory
                    .Include(x => x.Work)
                    .Where(x => x.WorkId == workId)
                    .OrderBy(x => x.Date)
                    .ToList();
            }
            else
            {
                data = _context.WorkHistory
                    .Include(x => x.Work)
                    .OrderBy(x => x.Date)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Observations?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Description?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Date.ToString()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Type?.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<WorkHistory>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<WorkHistory>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public WorkHistory GetById(int id)
        {
            return _context.WorkHistory
                .Include(x => x.Work)
                .FirstOrDefault(x => x.Id == id);
        }

        public WorkHistory Add(WorkHistory newWorkHistory)
        {
            _context.WorkHistory.Add(newWorkHistory);
            _context.SaveChanges();
            return newWorkHistory;
        }

        public bool Update(WorkHistory workHistory)
        {
            if (!WorkHistoryExists(workHistory.Id))
                return false;

            _context.WorkHistory.Update(workHistory);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!WorkHistoryExists(id))
                return false;

            var toRemove = _context.WorkHistory.Find(id);
            _context.WorkHistory.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}