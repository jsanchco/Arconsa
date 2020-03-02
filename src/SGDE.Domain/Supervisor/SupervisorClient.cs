namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Converters;
    using Entities;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public List<ClientViewModel> GetAllClient()
        {
            return ClientConverter.ConvertList(_clientRepository.GetAll());
        }

        public ClientViewModel GetClientById(int id)
        {
            var clientViewModel = ClientConverter.Convert(_clientRepository.GetById(id));

            return clientViewModel;
        }

        public ClientViewModel AddClient(ClientViewModel newClientViewModel)
        {
            var builder = new Client
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newClientViewModel.iPAddress,

                Name = newClientViewModel.name,
                Cif = newClientViewModel.cif,
                PhoneNumber = newClientViewModel.phoneNumber,
                Address = newClientViewModel.address
            };

            _clientRepository.Add(builder);
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

            return _clientRepository.Update(client);
        }

        public bool DeleteClient(int id)
        {
            return _clientRepository.Delete(id);
        }
    }
}
