namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

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

        public List<Work> GetAll()
        {
            return _context.Work
                .Include(x => x.Client)
                .ToList();
        }

        public Work GetById(int id)
        {
            return _context.Work
                .Include(x => x.Client)
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

            _context.Work.Update(work);
            _context.SaveChanges();
            return true;
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
