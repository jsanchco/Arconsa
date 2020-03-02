﻿namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class RoleRepository : IRoleRepository
    {
        private readonly EFContextMySQL _context;

        public RoleRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool RoleExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Role> GetAll()
        {
            return _context.Role
                .Include(x => x.Users)
                .ToList();
        }

        public Role GetById(int id)
        {
            return _context.Role
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == id);
        }

        public Role Add(Role newRole)
        {
            _context.Role.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }

        public bool Update(Role role)
        {
            if (!RoleExists(role.Id))
                return false;

            _context.Role.Update(role);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!RoleExists(id))
                return false;

            var toRemove = _context.Role.Find(id);
            _context.Role.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
