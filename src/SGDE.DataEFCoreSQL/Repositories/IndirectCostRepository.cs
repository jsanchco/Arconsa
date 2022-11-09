namespace SGDE.DataEFCoreSQL.Repositories
{
    #region Using

    using Domain.Entities;
    using Domain.Repositories;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class IndirectCostRepository : IIndirectCostRepository, IDisposable
    {
        private static readonly string[] MONTHS = { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
        private readonly EFContextSQL _context;

        public IndirectCostRepository(EFContextSQL context)
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

        private bool IndirectCostExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<IndirectCost> GetAll(int skip = 0, int take = 0, string filter = null)
        {
            var data = _context.IndirectCost
                .ToList();

            if (!string.IsNullOrEmpty(filter))
            {
                var filterSplit = filter.Split(",");
                if (filterSplit.Count() == 2)
                {
                    if (!Int32.TryParse(filterSplit[0].Trim(), out int year))
                        throw new Exception("Año mal configurado");

                    var month = filterSplit[1].Trim();
                    var index = Array.FindIndex(MONTHS, x => x == month.ToUpper());
                    if (index == -1)
                        throw new Exception("Mes mal configurado");

                    index++;
                    data = data.Where(x => x.Date >= new DateTime(year, index, 1) &&
                                           x.Date <= new DateTime(year, index, DateTime.DaysInMonth(year, index)))
                               .ToList();
                }
                else
                {
                    throw new Exception("Búsqueda mal configurada");
                }
            }

            var result = data
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.AccountNumber);

            var count = result.Count();
            return (skip != 0 || take != 0)
                ? new QueryResult<IndirectCost>
                {
                    Data = result.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<IndirectCost>
                {
                    Data = result.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public IndirectCost GetById(int id)
        {
            return _context.IndirectCost.FirstOrDefault(x => x.Id == id);
        }

        public IndirectCost Add(IndirectCost newIndirectCost)
        {
            _context.IndirectCost.Add(newIndirectCost);
            _context.SaveChanges();
            return newIndirectCost;
        }

        public bool Update(IndirectCost indirectCost)
        {
            if (!IndirectCostExists(indirectCost.Id))
                return false;

            _context.IndirectCost.Update(indirectCost);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!IndirectCostExists(id))
                return false;

            var toRemove = _context.IndirectCost.Find(id);
            _context.IndirectCost.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }

        public List<IndirectCost> GetAllInDate(DateTime date)
        {
            return _context.IndirectCost
                .Where(x => x.Date == date)
                .ToList();
        }

        public List<IndirectCost> GetAllBetweenDates(DateTime start, DateTime end)
        {
            return _context.IndirectCost
                .Where(x => x.Date >= start && x.Date <= end)
                .ToList();
        }
    }
}
