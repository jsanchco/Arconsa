namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;

    #endregion

    public class UserHiringRepository : IUserHiringRepository
    {
        private readonly EFContextMySQL _context;

        public UserHiringRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool UserHiringExists(int id)
        {
            return GetById(id) != null;
        }

        public List<UserHiring> GetAll(int userId = 0, int workId = 0)
        {
            if (userId == 0 && workId == 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .ToList();
            }

            if (userId != 0 && workId == 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }

            if (userId == 0 && workId != 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Where(x => x.WorkId == workId)
                    .ToList();
            }

            if (userId != 0 && workId != 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId && x.WorkId == workId)
                    .ToList();
            }

            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .ToList();
        }

        public List<UserHiring> GetOpen()
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .Where(x => x.EndDate == null)
                .ToList();
        }

        public UserHiring GetById(int id)
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public UserHiring Add(UserHiring newUserHiring)
        {
            _context.UserHiring.Add(newUserHiring);
            _context.SaveChanges();
            return newUserHiring;
        }

        public bool Update(UserHiring userHiring)
        {
            if (!UserHiringExists(userHiring.Id))
                return false;

            _context.UserHiring.Update(userHiring);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!UserHiringExists(id))
                return false;

            var toRemove = _context.Role.Find(id);
            _context.Role.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }

        public bool AssignWorkers(List<int> listUserId, int workId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var work = _context.Work
                        .Include(x => x.UserHirings)
                        .FirstOrDefault(x => x.Id == workId);
                    if (work == null)
                        throw new Exception("No existe esta obra");

                    var workersInWork = work.UserHirings
                        .ToList()
                        .Where(r => r.EndDate == null)?
                        .GroupBy(x => x.UserId).Select(y => y.First())
                        .Select(z => z.UserId);

                    if (workersInWork != null)
                    {
                        var removeWorkerId = workersInWork.Except(listUserId);
                        foreach (var remove in removeWorkerId)
                        {
                            var userHiring = _context.UserHiring
                                .FirstOrDefault(x => x.UserId == remove && x.WorkId == workId && x.EndDate == null);
                            if (userHiring == null)
                                throw new Exception("No existe esta UserHiring");

                            userHiring.EndDate = DateTime.Now;
                            _context.Update(userHiring);

                            _context.SaveChanges();
                        }
                    }

                    foreach (var userId in listUserId)
                    {
                        var user = _context.User
                            .Include(x => x.UserHirings)
                            .FirstOrDefault(x => x.Id == userId);
                        if (user == null)
                            throw new Exception("No existe este trabajador");

                        var userHiring = user.UserHirings.ToList().FirstOrDefault(x => x.EndDate == null);
                        if (userHiring.WorkId != workId)
                        {
                            userHiring.EndDate = DateTime.Now;
                            _context.UserHiring.Update(userHiring);

                            _context.UserHiring.Add(new UserHiring
                            {
                                AddedDate = DateTime.Now,
                                ModifiedDate = null,

                                StartDate = DateTime.Now,
                                EndDate = null,
                                WorkId = workId,
                                UserId = userId
                            });

                            user.WorkId = workId;
                            _context.User.Update(user);

                            _context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return true;
        }
    }
}
