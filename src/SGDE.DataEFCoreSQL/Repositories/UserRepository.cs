namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Domain.Helpers;
    using System;

    #endregion

    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public UserRepository(EFContextSQL context)
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

        private async Task<bool> UserExistsAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return await GetByIdAsync(id, ct) != null;
        }

        private bool UserExists(int id)
        {
            return GetById(id) != null;
        }

        public async Task<User> Authenticate(string username, string password, CancellationToken ct = default(CancellationToken))
        {
            return await _context.User
                .Include(x => x.Role)
                .Include(x => x.Client)
                .Include(x => x.Work)
                .Include(x => x.UserProfessions)
                .ThenInclude(y => y.Profession)
                .FirstOrDefaultAsync(x => x.Username == username && x.Password == password, ct);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            return await _context.User
                .Include(x => x.Role)
                .Include(x => x.Client)
                .Include(x => x.Work)
                .Include(x => x.UserProfessions)
                .ThenInclude(y => y.Profession)
                .ToListAsync(ct);
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return await _context.User
                .Include(x => x.Role)
                .Include(x => x.Client)
                .Include(x => x.Work)
                .Include(x => x.UserProfessions)
                .ThenInclude(y => y.Profession)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<User> AddAsync(User newUser, CancellationToken ct = default(CancellationToken))
        {
            _context.User.Add(newUser);
            await _context.SaveChangesAsync(ct);
            return newUser;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken ct = default(CancellationToken))
        {
            if (!await UserExistsAsync(user.Id, ct))
                return false;

            _context.User.Update(user);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            if (!await UserExistsAsync(id, ct))
                return false;

            var toRemove = _context.User.Find(id);
            _context.User.Remove(toRemove);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public QueryResult<User> GetAll(
            int skip = 0,
            int take = 0,
            string orderBy = null,
            int enterpriseId = 0,
            string filter = null,
            List<int> roles = null,
            bool showAllEmployees = true)
        {
            List<User> data;

            if (roles == null)
            {
                data = _context.User
                            .Include(x => x.Role)
                            .Include(x => x.Client)
                            .Include(x => x.Work)
                            .Include(x => x.Embargos)
                            .Include(x => x.UserProfessions)
                            .ThenInclude(y => y.Profession)
                            .Include(x => x.Advances)
                            .Where(x => x.EnterpriseId == enterpriseId)
                            .ToList();
            }
            else
            {
                data = showAllEmployees
                    ? _context.User
                                .Include(x => x.Role)
                                .Include(x => x.Client)
                                .Include(x => x.Work)
                                .Include(x => x.Embargos)
                                .Include(x => x.UserProfessions)
                                .ThenInclude(y => y.Profession)
                                .Include(x => x.SSHirings)
                                .Include(x => x.Advances)
                                .Where(x => x.EnterpriseId == enterpriseId && 
                                            roles.Contains(x.RoleId))
                                .ToList()
                    : _context.User
                                .Include(x => x.Role)
                                .Include(x => x.Client)
                                .Include(x => x.Work)
                                .Include(x => x.Embargos)
                                .Include(x => x.UserProfessions)
                                .ThenInclude(y => y.Profession)
                                .Include(x => x.SSHirings)
                                .Include(x => x.Advances)
                                .Where(x => x.EnterpriseId == enterpriseId &&
                                            roles.Contains(x.RoleId) &&
                                            x.SSHirings.Count > 0 &&
                                            x.SSHirings.Any(y => y.EndDate == null))
                                .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Dni?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.SecuritySocialNumber?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Email?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Observations?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.PhoneNumber?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Surname?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Username?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Role.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(string.Join(',', x.UserProfessions?.Select(y => y.Profession.Name.ToLower()))).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Work?.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Client?.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                var orderBySplit = orderBy.Split(" ");
                var ascending = orderBySplit.Length > 1 ? false : true;
                orderBy = orderBySplit[0];

                switch (orderBy)
                {
                    case "workName":
                        data = ascending ? data.OrderBy(x => x.Work?.Name).ToList() : data.OrderByDescending(x => x.Work?.Name).ToList();
                        break;
                    case "name":
                        data = ascending ? data.OrderBy(x => x.Name).ToList() : data.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "surname":
                        data = ascending ? data.OrderBy(x => x.Surname).ToList() : data.OrderByDescending(x => x.Surname).ToList();
                        break;
                    case "birthDate":
                        data = ascending ? data.OrderBy(x => x.BirthDate).ToList() : data.OrderByDescending(x => x.BirthDate).ToList();
                        break;
                }
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<User>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<User>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public List<User> GetUsersByEnterpriseId(int enterpriseId)
        {
            return _context.UserEnterprise
                .Include(x => x.User)
                .Include(x => x.Enterprise)
                .Where(x => x.EnterpriseId == enterpriseId)
                .Select(x => x.User)
                .ToList();
        }

        public List<User> GetUsersByRole(List<int> roles)
        {
            return _context.User
                .Include(x => x.Role)
                .Include(x => x.Work)
                .Include(x => x.UserProfessions)
                .ThenInclude(y => y.Profession)
                .Include(x => x.SSHirings)
                .Where(x => roles.Contains(x.RoleId))
                .ToList();
        }

        public List<User> GetWorkersWithSS(int enterpriseId)
        {
            return _context.User
                .Include(x => x.Work)
                .Include(x => x.SSHirings)
                .Include(x => x.UserHirings)
                .Where(x => x.EnterpriseId == enterpriseId &&
                            x.RoleId == 3 &&
                            x.SSHirings.Count > 0 &&
                            x.SSHirings.Any(y => y.EndDate == null))
                .ToList();
        }

        public User GetById(int id)
        {
            return _context.User
                .Include(x => x.Role)
                .Include(x => x.Client)
                .Include(x => x.Work)
                .Include(x => x.UserHirings)
                .Include(x => x.UserProfessions)
                .ThenInclude(y => y.Profession)
                .Include(x => x.SSHirings)
                .Include(x => x.Advances)
                .FirstOrDefault(x => x.Id == id);
        }

        public User Add(User newUser)
        {
            _context.User.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public User AddWithProfessions(User newUser, List<int> professions)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.User.Add(newUser);
                    _context.SaveChanges();

                    foreach (var profession in professions)
                    {
                        _context.UserProfession.Add(new UserProfession
                        {
                            UserId = newUser.Id,
                            ProfessionId = profession
                        });
                    }
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return newUser;
        }

        public bool Update(User user)
        {
            if (!UserExists(user.Id))
                return false;

            _context.User.Update(user);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!UserExists(id))
                return false;

            var professions = _context.UserProfession.Where(x => x.UserId == id);
            foreach (var profession in professions)
            {
                _context.UserProfession.Remove(profession);
            }

            var toRemove = _context.User.Find(id);
            _context.User.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }

        public long UsersTotalRegs()
        {
            return _context.User.Count();
        }

        public List<TypeDocument> GetPendingDocuments(int userId)
        {
            var userDocuments = _context.UserDocument
                .Include(x => x.TypeDocument)
                .Where(x => x.UserId == userId && x.File != null && x.TypeDocument.IsRequired == true)
                .Select(x => x.TypeDocument.Id);

            var documentsRequired = _context.TypeDocument.Where(x => x.IsRequired == true).Select(x => x.Id);

            var result = documentsRequired.Except(userDocuments);

            return _context.TypeDocument.Where(x => result.Contains(x.Id)).ToList();
        }
    }
}
