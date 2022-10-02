using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public static class LibraryConverter
    {
        public static LibraryViewModel Convert(Library library)
        {
            if (library == null)
                return null;

            var libraryViewModel = new LibraryViewModel
            {
                id = library.Id,
                addedDate = library.AddedDate,
                modifiedDate = library.ModifiedDate,
                iPAddress = library.IPAddress,

                reference = library.Reference,
                department = library.Department,
                description = library.Description,
                date = library.Date,
                edition = library.Edition,
                active = library.Active,
                typeFile = library.TypeFile,
                file = library.File    ,
                fileName = library.FileName
            };

            return libraryViewModel;
        }

        public static List<LibraryViewModel> ConvertList(IEnumerable<Library> libraries)
        {
            return libraries?.Select(library =>
            {
                var model = new LibraryViewModel
                {
                    id = library.Id,
                    addedDate = library.AddedDate,
                    modifiedDate = library.ModifiedDate,
                    iPAddress = library.IPAddress,

                    reference = library.Reference,
                    department = library.Department,
                    description = library.Description,
                    date = library.Date,
                    edition = library.Edition,
                    active = library.Active,
                    typeFile = library.TypeFile,
                    file = library.File,
                    fileName = library.FileName
                };
                return model;
            })
                .ToList();
        }
    }
}
