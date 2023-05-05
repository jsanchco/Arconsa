using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class CompanyDataRepository : ICompanyDataRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public CompanyDataRepository(EFContextSQL context)
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

        private bool CompanyDataExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<CompanyData> GetAll(int skip = 0, int take = 0, int enterpriseId = 0, string filter = null)
        {
            List<CompanyData> data;

            if (enterpriseId == 0)
            {
                data = _context.CompanyData
                    .OrderBy(x => x.Date)
                    .ToList();
            }
            else
            {
                data = _context.CompanyData
                    .Where(x => x.EnterpriseId == enterpriseId)
                    .OrderBy(x => x.Date)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Reference?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Description?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Observations?.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<CompanyData>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<CompanyData>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public CompanyData GetById(int id)
        {
            return _context.CompanyData
                .FirstOrDefault(x => x.Id == id);
        }

        public CompanyData Add(CompanyData newCompanyData)
        {
            _context.CompanyData.Add(newCompanyData);
            _context.SaveChanges();
            return newCompanyData;
        }

        public bool Update(CompanyData companyData)
        {
            if (!CompanyDataExists(companyData.Id))
                return false;

            _context.CompanyData.Update(companyData);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!CompanyDataExists(id))
                return false;

            var toRemove = _context.CompanyData.Find(id);
            _context.CompanyData.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
