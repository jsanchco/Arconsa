namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class TypeClientConverter
    {
        public static TypeClientViewModel Convert(TypeClient typeClient)
        {
            if (typeClient == null)
                return null;

            var typeClientViewModel = new TypeClientViewModel
            {
                id = typeClient.Id,
                addedDate = typeClient.AddedDate,
                modifiedDate = typeClient.ModifiedDate,
                iPAddress = typeClient.IPAddress,

                name = typeClient.Name,
                description = typeClient.Description
            };

            return typeClientViewModel;
        }

        public static List<TypeClientViewModel> ConvertList(IEnumerable<TypeClient> typeClients)
        {
            return typeClients?.Select(typeClient =>
            {
                var model = new TypeClientViewModel
                {
                    id = typeClient.Id,
                    addedDate = typeClient.AddedDate,
                    modifiedDate = typeClient.ModifiedDate,
                    iPAddress = typeClient.IPAddress,

                    name = typeClient.Name,
                    description = typeClient.Description
                };
                return model;
            })
                .ToList();
        }
    }
}
