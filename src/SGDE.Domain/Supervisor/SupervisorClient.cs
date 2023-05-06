namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using SGDE.Domain.Helpers;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public List<ClientViewModel> GetAllClientWithoutFilter()
        {
            return ClientConverter.ConvertList(_clientRepository.GetAllWithoutFilter());
        }

        public QueryResult<ClientViewModel> GetAllClient(int skip = 0, int take = 0, int enterpriseId = 0, bool allClients = true, string filter = null)
        {
            var queryResult = _clientRepository.GetAll(skip, take, enterpriseId, allClients, filter);
            return new QueryResult<ClientViewModel>
            {
                Data = ClientConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public ClientViewModel GetClientById(int id)
        {
            var clientViewModel = ClientConverter.Convert(_clientRepository.GetById(id));

            return clientViewModel;
        }

        public ClientViewModel AddClient(ClientViewModel newClientViewModel)
        {
            var client = new Client
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newClientViewModel.iPAddress,

                Name = newClientViewModel.name,
                Cif = newClientViewModel.cif,
                PhoneNumber = newClientViewModel.phoneNumber,
                Address = newClientViewModel.address,
                WayToPay = newClientViewModel.wayToPay,
                ExpirationDays = newClientViewModel.expirationDays,
                AccountNumber = newClientViewModel.accountNumber,
                Email = newClientViewModel.email,
                EmailInvoice = newClientViewModel.emailInvoice,
                Active = newClientViewModel.active,
                EnterpriseId = newClientViewModel.enterpriseId
            };

            _clientRepository.Add(client);
            return newClientViewModel;
        }

        public bool UpdateClient(ClientViewModel clientViewModel)
        {
            if (clientViewModel.id == null)
                return false;

            var client = _clientRepository.GetById((int)clientViewModel.id);

            if (client == null) return false;

            client.ModifiedDate = DateTime.Now;
            client.IPAddress = clientViewModel.iPAddress;

            client.Name = clientViewModel.name;
            client.Cif = clientViewModel.cif;
            client.PhoneNumber = clientViewModel.phoneNumber;
            client.Address = clientViewModel.address;
            client.WayToPay = clientViewModel.wayToPay;
            client.ExpirationDays = clientViewModel.expirationDays;
            client.AccountNumber = clientViewModel.accountNumber;
            client.Email = clientViewModel.email;
            client.EmailInvoice = clientViewModel.emailInvoice;
            client.Active = clientViewModel.active;
            client.EnterpriseId = clientViewModel.enterpriseId;

            return _clientRepository.Update(client);
        }

        public bool DeleteClient(int id)
        {
            return _clientRepository.Delete(id);
        }

        public List<ClientViewModel> GetAllClientLite(string filter = null)
        {
            return ClientConverter.ConvertList(_clientRepository.GetAllLite(filter));
        }
    }
}
