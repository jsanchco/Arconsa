namespace SGDE.DataEFCoreSQL.Repositories
{  
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using SGDE.Domain.Helpers;

    #endregion

    public class UserHiringRepository : IUserHiringRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public UserHiringRepository(EFContextSQL context)
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

        private bool UserHiringExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<UserHiring> GetAll(int skip = 0, int take = 0, string filter = null, int userId = 0, int workId = 0)
        {
            List<UserHiring> data = new List<UserHiring>();

            if (userId == 0 && workId == 0)
            {
                data = _context.UserHiring
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (userId != 0 && workId == 0)
            {
                data = _context.UserHiring
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (userId == 0 && workId != 0)
            {
                data = _context.UserHiring
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .Where(x => x.WorkId == workId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            if (userId != 0 && workId != 0)
            {
                data = _context.UserHiring
                    .Include(x => x.Work)
                    .ThenInclude(x => x.Client)
                    .Include(x => x.User)
                    .Include(x => x.Profession)
                    .Where(x => x.UserId == userId && x.WorkId == workId)
                    .OrderByDescending(x => x.StartDate)
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<UserHiring>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<UserHiring>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public List<UserHiring> GetAllByWorkId(int workId, bool actualWorking = false)
        {
            List<UserHiring> data = new List<UserHiring>();

            data = actualWorking ?
                _context.UserHiring
                    .Include(x => x.User)
                    .Where(x => x.WorkId == workId && x.EndDate == null)
                    .ToList() :
                _context.UserHiring
                    .Include(x => x.User)
                    .Where(x => x.WorkId == workId)
                    .ToList();

            return data;
        }

        public List<UserHiring> GetOpen()
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .ThenInclude(x => x.Client)
                .Include(x => x.User)
                .Include(x => x.Profession)
                .Where(x => x.EndDate == null)
                .ToList();
        }

        public UserHiring GetById(int id)
        {
            return _context.UserHiring
                .Include(x => x.Work)
                .ThenInclude(x => x.Client)
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
            var result = true;
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
                        foreach(var remove in removeWorkerId)
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
                            .Include(x => x.UserProfessions)
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
                                UserId = userId,
                                ProfessionId = user.UserProfessions.FirstOrDefault()?.ProfessionId
                            });
                            //if (result == true)
                            //    result = IsProfessionInClient(user.ProfessionId, 0, work.ClientId);

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
                                    UserId = userId,
                                    ProfessionId = user.UserProfessions.FirstOrDefault()?.ProfessionId
                                });
                                //if (result == true)
                                //    result = IsProfessionInClient(user.ProfessionId, 0, work.ClientId);

                                user.WorkId = workId;
                                _context.User.Update(user);

                                _context.SaveChanges();
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return result;
        }

        public bool IsProfessionInClient(int? professionId, int workId = 0, int clientId = 0)
        {
            if (professionId == null)
                return false;

            if (workId == 0 && clientId != 0)
            {
                var client = _context.Client
                                .Include(x => x.ProfessionInClients)
                                .FirstOrDefault(x => x.Id == clientId);
                if (client == null)
                    return false;

                if (client.ProfessionInClients.Select(x => x.ProfessionId).Contains((int)professionId))
                    return true;
                else
                    return false;
            }

            if (workId != 0 && clientId == 0)
            {
                var work = _context.Work
                                .Include(x => x.Client)
                                .ThenInclude(x => x.ProfessionInClients)
                                .FirstOrDefault(x => x.Id == workId);

                if (work.Client == null)
                    return false;

                if (work.Client.ProfessionInClients.Select(x => x.ProfessionId).Contains((int)professionId))
                    return true;
                else
                    return false;
            }

            if (workId != 0 && clientId != 0)
            {
                var client = _context.Client
                                .Include(x => x.ProfessionInClients)
                                .FirstOrDefault(x => x.Id == clientId);
                if (client == null)
                    return false;

                if (client.ProfessionInClients.Select(x => x.ProfessionId).Contains((int)professionId))
                    return true;
                else
                    return false;
            }

            return false;
        }

        private void ValidateUpdateUserHiring(UserHiring userHiring)
        {
            var listUserHiringsByUser = GetAll(userHiring.UserId);
            var openUserHiring = listUserHiringsByUser.Data.FirstOrDefault(x => x.EndDate == null && x.Id != userHiring.Id);
            if (openUserHiring != null && userHiring.EndDate == null)
            {
                throw new Exception("No se puede actualizar esta contratación, existe otra en uso");
            }
        }

        public UserHiring GetByWorkAndStartDateNull(int workId)
        {
            return _context.UserHiring
                    .FirstOrDefault(x => x.WorkId == workId && x.StartDate == null);
        }

        public UserHiring GetByWorkerAndEndDateNull(int workerId)
        {
            return _context.UserHiring
                    .FirstOrDefault(x => x.UserId == workerId && x.EndDate == null);
        }

        public List<UserHiring> GetByUserAndInWork(int userId, bool inWork)
        {
            return _context.UserHiring
                    .Where(x => x.UserId == userId && x.InWork == inWork)
                    .ToList();
        }
    }
}
