namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class TypeDocumentConverter
    {
        public static TypeDocumentViewModel Convert(TypeDocument typeDocument)
        {
            if (typeDocument == null)
                return null;

            var typeDocumentViewModel = new TypeDocumentViewModel
            {
                id = typeDocument.Id,
                addedDate = typeDocument.AddedDate,
                modifiedDate = typeDocument.ModifiedDate,
                iPAddress = typeDocument.IPAddress,

                name = typeDocument.Name,
                description = typeDocument.Description
            };

            return typeDocumentViewModel;
        }

        public static List<TypeDocumentViewModel> ConvertList(IEnumerable<TypeDocument> typeDocuments)
        {
            return typeDocuments?.Select(typeDocument =>
            {
                var model = new TypeDocumentViewModel
                {
                    id = typeDocument.Id,
                    addedDate = typeDocument.AddedDate,
                    modifiedDate = typeDocument.ModifiedDate,
                    iPAddress = typeDocument.IPAddress,

                    name = typeDocument.Name,
                    description = typeDocument.Description
                };
                return model;
            })
                .ToList();
        }
    }
}
