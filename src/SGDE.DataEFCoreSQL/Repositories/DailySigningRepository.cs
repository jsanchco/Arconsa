namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class DailySigningRepository : IDailySigningRepository
    {
        private readonly EFContextSQL _context;

        public DailySigningRepository(EFContextSQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool DailySigningExists(int id)
        {
            return GetById(id) != null;
        }

        public List<DailySigning> GetAll()
        {
            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .ToList();
        }

        public DailySigning GetById(int id)
        {
            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public DailySigning Add(DailySigning newDailySigning)
        {
            _context.DailySigning.Add(newDailySigning);
            _context.SaveChanges();
            return newDailySigning;
        }

        public bool Update(DailySigning dailySigning)
        {
            if (!DailySigningExists(dailySigning.Id))
                return false;

            _context.DailySigning.Update(dailySigning);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!DailySigningExists(id))
                return false;

            var toRemove = _context.DailySigning.Find(id);
            _context.DailySigning.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
