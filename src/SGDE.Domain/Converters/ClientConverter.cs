namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
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

                //idClient = string.IsNullOrEmpty(client.Cif) ? 
                //    string.Empty :
                //    client.Cif.Length < 5 ?
                //        client.Cif :
                //        client.Cif.Substring(client.Cif.Length - 5),

                name = client.Name,
                cif = client.Cif,
                address = client.Address,
                phoneNumber = client.PhoneNumber,
                wayToPay = client.WayToPay,
                expirationDays = client.ExpirationDays,
                accountNumber = client.AccountNumber,
                typeClientId = client.TypeClientId,
                typeClientName = client.TypeClient?.Name,
                email = client.Email,
                emailInvoice = client.EmailInvoice,
                active= client.Active
            };

            if (!string.IsNullOrEmpty(client.Cif))
            {
                var match = Regex.Match(client.Cif.Replace(".", string.Empty), @"\d+(?!\d+)");
                if (match.Success)
                {
                    if (match.Value.Length < 5)
                        clientViewModel.idClient = match.Value;
                    else
                        clientViewModel.idClient = match.Value.Substring(match.Value.Length - 5);
                }
                else
                    clientViewModel.idClient = client.Cif.Replace(".", string.Empty);
            }

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

                    //idClient = string.IsNullOrEmpty(client.Cif) ?
                    //string.Empty :
                    //client.Cif.Length < 5 ?
                    //    client.Cif :
                    //    client.Cif.Substring(client.Cif.Length - 5),

                    name = client.Name,
                    cif = client.Cif,
                    address = client.Address,
                    phoneNumber = client.PhoneNumber,
                    wayToPay = client.WayToPay,
                    expirationDays = client.ExpirationDays,
                    accountNumber = client.AccountNumber,
                    typeClientId = client.TypeClientId,
                    typeClientName = client.TypeClient?.Name,
                    email = client.Email,
                    emailInvoice = client.EmailInvoice,
                    active = client.Active
                };

                if (!string.IsNullOrEmpty(client.Cif))
                {
                    var match = Regex.Match(client.Cif.Replace(".", string.Empty), @"\d+(?!\d+)");
                    if (match.Success)
                    {
                        if (match.Value.Length < 5)
                            model.idClient = match.Value;
                        else
                            model.idClient = match.Value.Substring(match.Value.Length - 5);
                    }
                    else
                        model.idClient = client.Cif.Replace(".", string.Empty);
                }

                return model;
            })
                .ToList();
        }
    }
}