using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class WorkCostRepository : IWorkCostRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public WorkCostRepository(EFContextSQL context)
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

        private bool WorkCostExists(int id)
        {
            return GetById(id) != null;
        }

        public List<WorkCost> GetAll(int workId)
        {
            if (workId != 0)
            {
                return _context.WorkCost
                    .Include(x => x.Work)
                    .Where(x => x.WorkId == workId)
                    .OrderBy(x => x.Date)
                    .ToList();
            }

            return _context.WorkCost
                .Include(x => x.Work)
                .OrderBy(x => x.Date)
                .ToList();
        }

        public WorkCost GetById(int id)
        {
            return _context.WorkCost
                .Include(x => x.Work)
                .FirstOrDefault(x => x.Id == id);
        }

        public WorkCost Add(WorkCost newWorkCost)
        {
            _context.WorkCost.Add(newWorkCost);
            _context.SaveChanges();
            return newWorkCost;
        }

        public bool Update(WorkCost workCost)
        {
            if (!WorkCostExists(workCost.Id))
                return false;

            _context.WorkCost.Update(workCost);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!WorkCostExists(id))
                return false;

            var toRemove = _context.WorkCost.Find(id);
            _context.WorkCost.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}

