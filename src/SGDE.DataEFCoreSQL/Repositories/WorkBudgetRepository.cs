using Microsoft.EntityFrameworkCore;
using SGDE.Domain.Entities;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class WorkBudgetRepository : IWorkBudgetRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public WorkBudgetRepository(EFContextSQL context)
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

        private bool WorkBudgetExists(int id)
        {
            return GetById(id) != null;
        }

        public List<WorkBudget> GetAll(int workId)
        {
            if (workId != 0)
            {
                return _context.WorkBudget
                    .Include(x => x.Work)
                    .Where(x => x.WorkId == workId)
                    .OrderBy(x => x.Date)
                    .ToList();
            }

            return _context.WorkBudget
                .Include(x => x.Work)
                .OrderBy(x => x.Date)
                .ToList();
        }

        public WorkBudget GetById(int id)
        {
            return _context.WorkBudget
                .Include(x => x.Work)
                .FirstOrDefault(x => x.Id == id);
        }

        public WorkBudget Add(WorkBudget newWorkBudget)
        {
            _context.WorkBudget.Add(newWorkBudget);
            _context.SaveChanges();
            return newWorkBudget;
        }

        public bool Update(WorkBudget workBudget)
        {
            if (!WorkBudgetExists(workBudget.Id))
                return false;

            _context.WorkBudget.Update(workBudget);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!WorkBudgetExists(id))
                return false;

            var toRemove = _context.WorkBudget.Find(id);
            _context.WorkBudget.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }

        public List<WorkBudget> GetAllLite(int workId = 0)
        {
            List<WorkBudget> data;

            if (workId != 0)
            {
                data = _context.WorkBudget
                    .Where(x => x.WorkId == workId && x.Type != "Version X")
                    .Select(x => new WorkBudget
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }
            else
            {
                data = _context.WorkBudget
                    .Select(x => new WorkBudget
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }

            return data;
        }
    }
}