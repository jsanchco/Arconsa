﻿namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using SGDE.Domain.Helpers;
    using SGDE.Domain.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class CostWorkerRepository : ICostWorkerRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public CostWorkerRepository(EFContextMySQL context)
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

        private bool CostWorkerExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<CostWorker> GetAll(int skip = 0, int take = 0, string filter = null, int userId = 0)
        {
            var data = new List<CostWorker>();

            if (userId == 0)
            {
                data = _context.CostWorker
                        .Include(x => x.User)
                        .OrderByDescending(x => x.StartDate)
                        .ToList();
            }
            if (userId != 0)
            {
                data = _context.CostWorker
                        .Include(x => x.User)
                        .Where(x => x.UserId == userId)
                        .OrderByDescending(x => x.StartDate)
                        .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<CostWorker>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<CostWorker>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public List<CostWorker> GetCostWorkerBetweenDates(DateTime startDate, DateTime endDate)
        {
            return _context.CostWorker
                .Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                .ToList();
        }

        public CostWorker GetById(int id)
        {
            return _context.CostWorker
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public CostWorker Add(CostWorker newCostWorker)
        {
            _context.CostWorker.Add(newCostWorker);
            _context.SaveChanges();
            return newCostWorker;
        }

        public bool Update(CostWorker costWorker)
        {
            if (!CostWorkerExists(costWorker.Id))
                return false;

            _context.CostWorker.Update(costWorker);
            _context.SaveChanges();
            return true;
        }

        public bool ValidateCostWorker(CostWorker costWorker)
        {
            if (costWorker.StartDate > costWorker.EndDate)
                return false;

            if (_context.CostWorker.FirstOrDefault(x =>
                x.Id != costWorker.Id &&
                x.UserId == costWorker.UserId &&
                x.EndDate == null &&
                costWorker.EndDate == null) != null)
                return false;

            if (_context.CostWorker.FirstOrDefault(x =>
                x.Id != costWorker.Id &&
                x.UserId == costWorker.UserId &&
                x.EndDate == null &&
                (costWorker.EndDate >= x.StartDate || costWorker.EndDate >= x.StartDate)) != null)
                return false;

            if (_context.CostWorker.FirstOrDefault(x =>
                x.Id != costWorker.Id &&
                x.UserId == costWorker.UserId &&
                x.UserId == costWorker.UserId &&
                x.EndDate != null &&
                (costWorker.StartDate <= x.StartDate && costWorker.EndDate == null)) != null)
                return false;

            if (_context.CostWorker.FirstOrDefault(x =>
                x.Id != costWorker.Id &&
                x.UserId == costWorker.UserId &&
                x.EndDate != null &&
                (costWorker.StartDate >= x.StartDate && costWorker.StartDate <= x.EndDate)) != null)
                return false;

            if (_context.CostWorker.FirstOrDefault(x =>
                x.Id != costWorker.Id &&
                x.UserId == costWorker.UserId &&
                x.EndDate != null &&
                (costWorker.EndDate >= x.StartDate && costWorker.EndDate <= x.EndDate)) != null)
                return false;

            if (_context.CostWorker.FirstOrDefault(x =>
                x.Id != costWorker.Id &&
                x.UserId == costWorker.UserId &&
                x.EndDate != null &&
                (costWorker.StartDate <= x.StartDate && costWorker.EndDate >= x.EndDate)) != null)
                return false;

            return true;
        }

        public bool Delete(int id)
        {
            if (!CostWorkerExists(id))
                return false;

            var toRemove = _context.CostWorker.Find(id);
            _context.CostWorker.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
