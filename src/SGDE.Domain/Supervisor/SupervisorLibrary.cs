using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<LibraryViewModel> GetAllLibrary(int skip = 0, int take = 0, string filter = null)
        {
            var queryResult = _libraryRepository.GetAll(skip, take, filter);
            return new QueryResult<LibraryViewModel>
            {
                Data = LibraryConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public LibraryViewModel GetLibraryById(int id)
        {
            var libraryViewModel = LibraryConverter.Convert(_libraryRepository.GetById(id));

            return libraryViewModel;
        }

        public LibraryViewModel AddLibrary(LibraryViewModel newLibraryViewModel)
        {
            var library = new Library
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newLibraryViewModel.iPAddress,

                Reference = newLibraryViewModel.reference,
                Department = newLibraryViewModel.department,
                Description = newLibraryViewModel.description,
                Date = newLibraryViewModel.date,
                Edition = newLibraryViewModel.edition,
                Active = newLibraryViewModel.active,
                File = newLibraryViewModel.file,
                TypeFile = newLibraryViewModel.typeFile,
                FileName = newLibraryViewModel.fileName
            };

            _libraryRepository.Add(library);
            return newLibraryViewModel;
        }

        public bool UpdateLibrary(LibraryViewModel libraryViewModel)
        {
            if (libraryViewModel.id == null)
                return false;

            var library = _libraryRepository.GetById((int)libraryViewModel.id);

            if (library == null) return false;

            library.ModifiedDate = DateTime.Now;
            library.IPAddress = libraryViewModel.iPAddress;

            library.Reference = libraryViewModel.reference;
            library.Department = libraryViewModel.department;
            library.Description = libraryViewModel.description;
            library.Date = libraryViewModel.date;
            library.Edition = libraryViewModel.edition;
            library.Active = libraryViewModel.active;
            library.File = libraryViewModel.file;
            library.TypeFile = libraryViewModel.typeFile;
            library.FileName = libraryViewModel.fileName;

            return _libraryRepository.Update(library);
        }

        public bool DeleteLibrary(int id)
        {
            return _libraryRepository.Delete(id);
        }
    }
}
