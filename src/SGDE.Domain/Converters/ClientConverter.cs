namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class ClientConverter
    {
        public static ClientViewModel Convert(Client client)
        {
            if (client == null)
                return null;

            var clientViewModel = new ClientViewModel
            {
                id = client.Id,
                addedDate = client.AddedDate,
                modifiedDate = client.ModifiedDate,
                iPAddress = client.IPAddress,

                name = client.Name,
                cif = client.Cif,
                address = client.Address,
                phoneNumber = client.PhoneNumber,
                typeClientId = client.TypeClientId,
                typeClientName = client.TypeClient?.Name
            };

            return clientViewModel;
        }

        public static List<ClientViewModel> ConvertList(IEnumerable<Client> clients)
        {
            return clients?.Select(client =>
            {
                var model = new ClientViewModel
                {
                    id = client.Id,
                    addedDate = client.AddedDate,
                    modifiedDate = client.ModifiedDate,
                    iPAddress = client.IPAddress,

                    name = client.Name,
                    cif = client.Cif,
                    address = client.Address,
                    phoneNumber = client.PhoneNumber,
                    typeClientId = client.TypeClientId,
                    typeClientName = client.TypeClient?.Name
                };
                return model;
            })
                .ToList();
        }
    }
}