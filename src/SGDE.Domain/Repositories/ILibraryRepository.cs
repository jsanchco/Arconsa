using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;

namespace SGDE.Domain.Repositories
{
    public interface ILibraryRepository
    {
        QueryResult<Library> GetAll(int skip = 0, int take = 0);
        Library GetById(int id);
        Library Add(Library newLibrary);
        bool Update(Library library);
        bool Delete(int id);
    }
}
