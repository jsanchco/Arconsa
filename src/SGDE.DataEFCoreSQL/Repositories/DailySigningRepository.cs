namespace SGDE.DataEFCoreSQL.Repositories
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

    public class DailySigningRepository : IDailySigningRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public DailySigningRepository(EFContextSQL context)
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
                .Include(x => x.Profession)
                .OrderByDescending(x => x.StartHour)
                .ToList();
            }
            else
            {
                data = _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .Include(x => x.Profession)
                .Where(x => x.UserHiring.User.Id == userId)
                .OrderByDescending(x => x.StartHour)
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

        public List<DailySigning> GetHistoryByUserId(int userId)
        {
            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .ThenInclude(v => v.Client)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .Include(x => x.Profession)
                .Where(x => x.UserHiring.User.Id == userId)
                .OrderBy(x => x.StartHour)
                .ToList();
        }

        public List<DailySigning> GetHistoryByWorkId(int workId)
        {
            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .ThenInclude(v => v.Client)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .Include(x => x.Profession)
                .Where(x => x.UserHiring.Work.Id == workId)
                .OrderBy(x => x.StartHour)
                .ToList();
        }

        public DailySigning GetById(int id)
        {
            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(y => y.Work)
                .Include(z => z.UserHiring)
                .ThenInclude(w => w.User)
                .Include(x => x.Profession)
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
            if (dailySigning.HourTypeId == 5)
                return true;

            if (dailySigning.StartHour >= dailySigning.EndHour)
                return false;

            if (_context.DailySigning.FirstOrDefault(x =>
                x.StartHour < dailySigning.StartHour &&
                x.EndHour > dailySigning.StartHour &&
                x.Id != dailySigning.Id &&
                x.UserHiringId == dailySigning.UserHiringId) != null)
                return false;

            if (_context.DailySigning.FirstOrDefault(x =>
                x.StartHour < dailySigning.EndHour &&
                x.EndHour > dailySigning.StartHour &&
                x.Id != dailySigning.Id &&
                x.UserHiringId == dailySigning.UserHiringId) != null)
                return false;

            if (_context.DailySigning.FirstOrDefault(x =>
                x.StartHour < dailySigning.StartHour &&
                x.EndHour > dailySigning.EndHour &&
                x.Id != dailySigning.Id &&
                x.UserHiringId == dailySigning.UserHiringId) != null)
                return false;

            return true;
        }

        public List<DailySigning> GetByUserId(string startDate, string endDate, int userId)
        {
            var dtStart = DateTime.ParseExact(startDate, "d/MM/yyyy", null);
            var dtEnd = DateTime.ParseExact(endDate, "d/MM/yyyy", null).AddHours(23).AddMinutes(59);

            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work)
                .ThenInclude(x => x.Client)
                .ThenInclude(x => x.ProfessionInClients)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work.Client)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.CostWorkers)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Profession)
                .Include(x => x.HourType)
                .Include(x => x.Profession)
                .Where(x => x.StartHour >= dtStart && x.EndHour <= dtEnd && x.UserHiring.UserId == userId)
                .OrderBy(x => x.StartHour)
                .ToList();
        }

        public List<DailySigning> GetByWorkId(string startDate, string endDate, int workId)
        {
            var dtStart = DateTime.ParseExact(startDate, "d/MM/yyyy", null);
            var dtEnd = DateTime.ParseExact(endDate, "d/MM/yyyy", null).AddHours(23).AddMinutes(59);

            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work)
                .ThenInclude(x => x.Client)
                .ThenInclude(x => x.ProfessionInClients)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work.Client)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.CostWorkers)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Profession)
                .Include(x => x.HourType)
                .Include(x => x.Profession)
                .Where(x => x.StartHour >= dtStart && x.EndHour <= dtEnd && x.UserHiring.WorkId == workId)
                .ToList();

        }

        public List<DailySigning> GetByClientId(string startDate, string endDate, int clientId)
        {
            var dtStart = DateTime.ParseExact(startDate, "d/MM/yyyy", null);
            var dtEnd = DateTime.ParseExact(endDate, "d/MM/yyyy", null).AddHours(23).AddMinutes(59);

            return _context.DailySigning
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work)
                .ThenInclude(x => x.Client)
                .ThenInclude(x => x.ProfessionInClients)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Work.Client)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.CostWorkers)
                .Include(x => x.UserHiring)
                .ThenInclude(x => x.Profession)
                .Include(x => x.HourType)
                .Include(x => x.Profession)
                .Where(x => x.StartHour >= dtStart && x.EndHour <= dtEnd && x.UserHiring.Work.ClientId == clientId)
                .ToList();
        }
    }
}
