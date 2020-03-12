namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Domain.Helpers;
    using System;

    #endregion

    public class DailySigningRepository : IDailySigningRepository
    {
        private readonly EFContextMySQL _context;

        public DailySigningRepository(EFContextMySQL context)
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

        public QueryResult<DailySigning> GetAll(int skip = 0, int take = 0, int userId = 0)
        {
            List<DailySigning> data;

            if (userId == 0)
            {
                data = _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .ToList();
            }
            else
            {
                data = _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .Where(x => x.UserHiring.User.Id == userId)
                .ToList();
            }
            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<DailySigning>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<DailySigning>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public DailySigning GetById(int id)
        {
            return _context.DailySigning
                .Include(x => x.UserHiring)
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

        public bool ValidateDalilySigning(DailySigning dailySigning)
        {
            if (dailySigning.StartHour >= dailySigning.EndHour)
                return false;

            if (_context.DailySigning.FirstOrDefault(x => x.StartHour <= dailySigning.StartHour && x.EndHour >= dailySigning.StartHour) != null)
                return false;

            if (_context.DailySigning.FirstOrDefault(x => x.StartHour <= dailySigning.EndHour && x.EndHour >= dailySigning.StartHour) != null)
                return false;

            if (_context.DailySigning.FirstOrDefault(x => x.StartHour <= dailySigning.StartHour && x.EndHour >= dailySigning.EndHour) != null)
                return false;

            return true;
        }

        public List<DailySigning> GetByUserId(string startDate, string endDate, int userId)
        {
            var dtStart = DateTime.ParseExact(startDate, "d/MM/yyyy", null);
            var dtEnd = DateTime.ParseExact(endDate, "d/MM/yyyy", null);

            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work.Client)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.User)
                .Where(x => x.UserHiring.StartDate >= dtStart && x.UserHiring.EndDate <= dtEnd && x.UserHiring.UserId == userId)
                .ToList();
        }

        public List<DailySigning> GetByWorkId(string startDate, string endDate, int workId)
        {
            var dtStart = DateTime.ParseExact(startDate, "d/MM/yyyy", null);
            var dtEnd = DateTime.ParseExact(endDate, "d/MM/yyyy", null);

            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work.Client)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.User)
                .Where(x => x.UserHiring.StartDate >= dtStart && x.UserHiring.EndDate <= dtEnd && x.UserHiring.WorkId == workId)
                .ToList();

        }

        public List<DailySigning> GetByClientId(string startDate, string endDate, int clientId)
        {
            var dtStart = DateTime.ParseExact(startDate, "d/MM/yyyy", null);
            var dtEnd = DateTime.ParseExact(endDate, "d/MM/yyyy", null);

            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work.Client)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.User)
                .Where(x => x.UserHiring.StartDate >= dtStart && x.UserHiring.EndDate <= dtEnd && x.UserHiring.Work.ClientId == clientId)
                .ToList();
        }
    }
}

