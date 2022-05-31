using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreMySQL.Repositories
{
    public class WorkBudgetDataRepository : IWorkBudgetDataRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public WorkBudgetDataRepository(EFContextMySQL context)
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

        private bool WorkBudgetDataExists(int id)
        {
            return GetById(id) != null;
        }

        public List<WorkBudgetData> GetAll(int workId)
        {
            if (workId != 0)
            {
                return _context.WorkBudgetData
                    .Include(x => x.Work)
                    .Include(x => x.WorkBudgets)
                    .Where(x => x.WorkId == workId)
                    .OrderBy(x => x.AddedDate)
                    .ToList();
            }

            return _context.WorkBudgetData
                .Include(x => x.Work)
                .Include(x => x.WorkBudgets)
                .OrderBy(x => x.AddedDate)
                .ToList();
        }

        public WorkBudgetData GetById(int id)
        {
            return _context.WorkBudgetData
                .Include(x => x.Work)
                .Include(x => x.WorkBudgets)
                .FirstOrDefault(x => x.Id == id);
        }

        public WorkBudgetData GetByWorkIdAndReference(int workId, string reference)
        {
            return _context.WorkBudgetData
                .FirstOrDefault(x => x.WorkId == workId && x.Reference == reference);
        }

        public WorkBudgetData Add(WorkBudgetData newWorkBudgetData)
        {
            _context.WorkBudgetData.Add(newWorkBudgetData);
            _context.SaveChanges();
            return newWorkBudgetData;
        }

        public bool Update(WorkBudgetData workBudgetData)
        {
            if (!WorkBudgetDataExists(workBudgetData.Id))
                return false;

            _context.WorkBudgetData.Update(workBudgetData);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!WorkBudgetDataExists(id))
                return false;

            var toRemove = _context.WorkBudgetData.Find(id);
            _context.WorkBudgetData.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}