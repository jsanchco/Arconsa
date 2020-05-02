namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class TrainingRepository : ITrainingRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public TrainingRepository(EFContextMySQL context)
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

        private bool TrainingExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Training> GetAll(int userId)
        {
            if (userId != 0)
            {
                return _context.Training
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .ToList();

            }
            return _context.Training
                .Include(x => x.User)
                .ToList();
        }

        public Training GetById(int id)
        {
            return _context.Training
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public Training Add(Training newTraining)
        {
            _context.Training.Add(newTraining);
            _context.SaveChanges();
            return newTraining;
        }

        public bool Update(Training training)
        {
            if (!TrainingExists(training.Id))
                return false;

            _context.Training.Update(training);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!TrainingExists(id))
                return false;

            var toRemove = _context.Training.Find(id);
            _context.Training.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
