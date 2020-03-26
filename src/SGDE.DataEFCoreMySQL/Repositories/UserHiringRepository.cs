﻿namespace SGDE.DataEFCoreMySQL.Repositories
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
                    .Include(x => x.Profession)
                    .ToList();
            }

            if (userId != 0 && workId == 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }

            if (userId == 0 && workId != 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .Where(x => x.WorkId == workId)
                    .ToList();
            }

            if (userId != 0 && workId != 0)
            {
                return _context.UserHiring
                    .Include(x => x.Work)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .Where(x => x.UserId == userId && x.WorkId == workId)
                    .ToList();
            }

            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .Include(x => x.Profession)
                .ToList();
        }

        public List<UserHiring> GetOpen()
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .Include(x => x.Profession)
                .Where(x => x.EndDate == null)
                .ToList();
        }

        public UserHiring GetById(int id)
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .Include(x => x.User)
                .Include(x => x.Profession)
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

            ValidateUpdateUserHiring(userHiring);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.UserHiring.Update(userHiring);

                    var user = _context.User.FirstOrDefault(x => x.Id == userHiring.UserId);
                    if (user == null) return false;

                    if (userHiring.EndDate == null)
                    {
                        user.WorkId = userHiring.WorkId;
                    }
                    else
                    {
                        user.WorkId = null;
                    }
                    _context.User.Update(user);

                    _context.SaveChanges();

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

        public bool Delete(int id)
        {
            if (!UserHiringExists(id))
                return false;

            var toRemove = _context.UserHiring.Find(id);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.UserHiring.Remove(toRemove);

                    var user = _context.User.FirstOrDefault(x => x.Id == toRemove.UserId);
                    if (user == null) return false;

                    if (toRemove.EndDate == null)
                    {
                        user.WorkId = null;
                        _context.User.Update(user);
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


                    // Damos de baja los tragajadores que no estén incluidos en los seleccionados
                    var workersInWork = work.UserHirings
                        .ToList()
                        .Where(r => r.EndDate == null)?
                        .GroupBy(x => x.UserId).Select(y => y.First())
                        .Select(z => z.UserId);

                    if (workersInWork.Any())
                    {
                        var removeWorkerId = workersInWork.Except(listUserId);
                        foreach (var remove in removeWorkerId)
                        {
                            var userHiring = _context.UserHiring
                                .FirstOrDefault(x => x.UserId == remove && x.WorkId == workId && x.EndDate == null);
                            if (userHiring == null)
                                throw new Exception("No existe esta UserHiring");

                            userHiring.EndDate = DateTime.Now;
                            _context.UserHiring.Update(userHiring);

                            var user = _context.User
                                .Include(x => x.UserHirings)
                                .Include(x => x.Work)
                                .FirstOrDefault(x => x.Id == remove);
                            if (user == null)
                                throw new Exception("No existe este trabajador");

                            user.Work = null;
                            _context.User.Update(user);

                            _context.SaveChanges();
                        }
                    }
                    // Damos de baja los tragajadores que no estén incluidos en los seleccionados


                    foreach (var userId in listUserId)
                    {
                        var user = _context.User
                            .Include(x => x.UserHirings)
                            .Include(x => x.Work)
                            .FirstOrDefault(x => x.Id == userId);
                        if (user == null)
                            throw new Exception("No existe este trabajador");


                        if (user.Work == null)
                        {
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
                        else
                        {
                            var userHiring = user.UserHirings.ToList().FirstOrDefault(x => x.EndDate == null && x.WorkId != workId);
                            if (userHiring != null && user.WorkId != workId)
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

        private void ValidateUpdateUserHiring(UserHiring userHiring)
        {
            var listUserHiringsByUser = GetAll(userHiring.UserId);
            var openUserHiring = listUserHiringsByUser.FirstOrDefault(x => x.EndDate == null && x.Id != userHiring.Id);
            if (openUserHiring != null && userHiring.EndDate == null)
            {
                throw new Exception("No se puede actualizar esta contratación, existe otra en uso");
            }
        }
    }
}
