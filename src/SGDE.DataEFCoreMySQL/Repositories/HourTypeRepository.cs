namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class HourTypeRepository : IHourTypeRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public HourTypeRepository(EFContextMySQL context)
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
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        private bool HourTypeExists(int id)
        {
            return GetById(id) != null;
        }

        public List<HourType> GetAll()
        {
            return _context.HourType
                .ToList();
        }

        public HourType GetById(int id)
        {
            return _context.HourType
                .FirstOrDefault(x => x.Id == id);
        }

        public HourType Add(HourType newHourType)
        {
            _context.HourType.Add(newHourType);
            _context.SaveChanges();
            return newHourType;
        }

        public bool Update(HourType hourType)
        {
            if (!HourTypeExists(hourType.Id))
                return false;

            _context.HourType.Update(hourType);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!HourTypeExists(id))
                return false;

            var toRemove = _context.HourType.Find(id);
            _context.HourType.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
