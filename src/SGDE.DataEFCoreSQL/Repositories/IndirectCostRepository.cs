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

    public class IndirectCostRepository : IIndirectCostRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public IndirectCostRepository(EFContextSQL context)
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

        private bool IndirectCostExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<IndirectCost> GetAll(int skip = 0, int take = 0)
        {
            var data = _context.IndirectCost
                .ToList()
                .OrderByDescending(x => x.Key);

            var count = data.Count();
            return (skip != 0 || take != 0)
                ? new QueryResult<IndirectCost>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<IndirectCost>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public IndirectCost GetById(int id)
        {
            return _context.IndirectCost.FirstOrDefault(x => x.Id == id);
        }

        public IndirectCost Add(IndirectCost newIndirectCost)
        {
            _context.IndirectCost.Add(newIndirectCost);
            _context.SaveChanges();
            return newIndirectCost;
        }

        public bool Update(IndirectCost indirectCost)
        {
            if (!IndirectCostExists(indirectCost.Id))
                return false;

            _context.IndirectCost.Update(indirectCost);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!IndirectCostExists(id))
                return false;

            var toRemove = _context.IndirectCost.Find(id);
            _context.IndirectCost.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
