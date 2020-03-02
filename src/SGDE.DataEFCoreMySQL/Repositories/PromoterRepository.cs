﻿namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class PromoterRepository : IPromoterRepository
    {
        private readonly EFContextMySQL _context;

        public PromoterRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool PromoterExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Promoter> GetAll()
        {
            return _context.Promoter
                .Include(x => x.PromoterResponsibles)
                .ToList();
        }

        public Promoter GetById(int id)
        {
            return _context.Promoter
                .Include(x => x.PromoterResponsibles)
                .FirstOrDefault(x => x.Id == id);
        }

        public Promoter Add(Promoter newPromoter)
        {
            _context.Promoter.Add(newPromoter);
            _context.SaveChanges();
            return newPromoter;
        }

        public bool Update(Promoter promoter)
        {
            if (!PromoterExists(promoter.Id))
                return false;

            _context.Promoter.Update(promoter);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!PromoterExists(id))
                return false;

            var toRemove = _context.Promoter.Find(id);
            _context.Promoter.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
