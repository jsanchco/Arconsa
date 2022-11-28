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

    public class WorkStatusHistoryRepository : IWorkStatusHistoryRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public WorkStatusHistoryRepository(EFContextSQL context)
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

        private bool WorkStatusHistoryExists(int id)
        {
            return GetById(id) != null;
        }

        public List<WorkStatusHistory> GetAll(int workId)
        {
            List<WorkStatusHistory> data;

            if (workId == 0)
            {
                data = _context.WorkStatusHistory
                        .Include(x => x.Work)
                        .ToList();
            }
            else
            {
                data = _context.WorkStatusHistory
                        .Include(x => x.Work)
                        .Where(x => x.WorkId == workId)
                        .ToList();
            }

            return data;
        }

        public WorkStatusHistory GetById(int id)
        {
            var result = _context.WorkStatusHistory
                .Include(x => x.Work)
                .FirstOrDefault(x => x.Id == id);

            return result;
        }

        public WorkStatusHistory Add(WorkStatusHistory newWorkStatusHistory)
        {
            _context.WorkStatusHistory.Add(newWorkStatusHistory);
            _context.SaveChanges();
            return newWorkStatusHistory;
        }

        public bool Update(WorkStatusHistory workStatusHistory)
        {
            if (!WorkStatusHistoryExists(workStatusHistory.Id))
                return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var workOld = GetById(workStatusHistory.Id);

                    _context.WorkStatusHistory.Update(workStatusHistory);
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
            if (!WorkStatusHistoryExists(id))
                return false;

            var toRemove = _context.WorkStatusHistory.Find(id);
            _context.WorkStatusHistory.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
