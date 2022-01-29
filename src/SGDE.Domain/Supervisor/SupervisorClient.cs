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

        public QueryResult<ClientViewModel> GetAllClient(int skip = 0, int take = 0, string filter = null)
        {
            var queryResult = _clientRepository.GetAll(skip, take, filter);
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
                AccountNumber = newClientViewModel.accountNumber
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
            client.AccountNumber = clientViewModel.accountNumber;

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
