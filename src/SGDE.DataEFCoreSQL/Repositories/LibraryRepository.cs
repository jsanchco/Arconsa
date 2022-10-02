using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.DataEFCoreSQL.Repositories
{
    public class LibraryRepository : ILibraryRepository, IDisposable
    {
        private readonly EFContextSQL _context;

        public LibraryRepository(EFContextSQL context)
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

        private bool LibraryExists(int id)
        {
            return GetById(id) != null;
        }

        public QueryResult<Library> GetAll(int skip = 0, int take = 00)
        {
            List<Library> data;

            data = _context.Library
                .OrderBy(x => x.Date)
                .ToList();

            var count = data.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<Library>
                {
                    Data = data.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<Library>
                {
                    Data = data.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public Library GetById(int id)
        {
            return _context.Library
                .FirstOrDefault(x => x.Id == id);
        }

        public Library Add(Library newLibrary)
        {
            _context.Library.Add(newLibrary);
            _context.SaveChanges();
            return newLibrary;
        }

        public bool Update(Library library)
        {
            if (!LibraryExists(library.Id))
                return false;

            _context.Library.Update(library);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!LibraryExists(id))
                return false;

            var toRemove = _context.Library.Find(id);
            _context.Library.Remove(toRemove);
            _context.SaveChanges();
            return true;
        }
    }
}
