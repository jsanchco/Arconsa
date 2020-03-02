namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class UserDocumentConverter
    {
        public static UserDocumentViewModel Convert(UserDocument userDocument)
        {
            if (userDocument == null)
                return null;

            var userDocumentViewModel = new UserDocumentViewModel
            {
                id = userDocument.Id,
                addedDate = userDocument.AddedDate,
                modifiedDate = userDocument.ModifiedDate,
                iPAddress = userDocument.IPAddress,

                description = userDocument.Description,
                observations = userDocument.Observations,
                file = userDocument.File,
                typeDocumentId = userDocument.TypeDocumentId,
                typeDocumentName = userDocument.TypeDocument.Name,
                userId = userDocument.UserId,
                userUserName = userDocument.User.Username
            };

            return userDocumentViewModel;
        }

        public static List<UserDocumentViewModel> ConvertList(IEnumerable<UserDocument> userDocuments)
        {
            return userDocuments?.Select(userDocument =>
            {
                var model = new UserDocumentViewModel
                {
                    id = userDocument.Id,
                    addedDate = userDocument.AddedDate,
                    modifiedDate = userDocument.ModifiedDate,
                    iPAddress = userDocument.IPAddress,

                    description = userDocument.Description,
                    observations = userDocument.Observations,
                    file = userDocument.File,
                    typeDocumentId = userDocument.TypeDocumentId,
                    typeDocumentName = userDocument.TypeDocument?.Name,
                    userId = userDocument.UserId,
                    userUserName = userDocument.User.Username
                };
                return model;
            })
                .ToList();
        }
    }
}
