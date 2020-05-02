namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using SGDE.Domain.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public class ProfessionRepository : IProfessionRepository, IDisposable
    {
        private readonly EFContextMySQL _context;

        public ProfessionRepository(EFContextMySQL context)
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

        public async Task<List<Profession>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            return await _context.Profession
                .Include(x => x.Users)
                .ToListAsync(ct);
        }

        public async Task<Profession> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return await _context.Profession
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<List<User>> GetByProfesionIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return await _context.User
                .Include(x => x.Profession)
                .Where(x => x.ProfessionId == id)
                .ToListAsync(ct);
        }

        public async Task<Profession> AddAsync(Profession newProfession, CancellationToken ct = default(CancellationToken))
        {
            _context.Profession.Add(newProfession);
            await _context.SaveChangesAsync(ct);
            return newProfession;
        }

        public async Task<bool> UpdateAsync(Profession profession, CancellationToken ct = default(CancellationToken))
        {
            if (await GetByIdAsync(profession.Id, ct) == null)
                return false;

            _context.Profession.Update(profession);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            if (await GetByIdAsync(id, ct) == null)
                return false;

            var toRemove = _context.Profession.Find(id);
            _context.Profession.Remove(toRemove);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public List<Profession> GetAll()
        {
            return _context.Profession
                .Include(x => x.Users)
                .ToList();
        }

        public Profession GetById(int id)
        {
            return _context.Profession
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<User> GetByProfesionId(int id)
        {
            return _context.User
                .Include(x => x.Profession)
                .Where(x => x.ProfessionId == id)
                .ToList();
        }

        public Profession Add(Profession newProfession)
        {
            _context.Profession.Add(newProfession);
            _context.SaveChanges();
            return newProfession;
        }

        public bool Update(Profession profession)
        {
            if (GetById(profession.Id) == null)
                return false;

            _context.Profession.Update(profession);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (GetById(id) == null)
                return false;

            var toRemove = _context.Profession.Find(id);
            _context.Profession.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
