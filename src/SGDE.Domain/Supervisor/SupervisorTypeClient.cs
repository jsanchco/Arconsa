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
        public List<TypeClientViewModel> GetAllTypeClient()
        {
            return TypeClientConverter.ConvertList(_typeClientRepository.GetAll());
        }

        public TypeClientViewModel GetTypeClientById(int id)
        {
            var typeClientViewModel = TypeClientConverter.Convert(_typeClientRepository.GetById(id));

            return typeClientViewModel;
        }

        public TypeClientViewModel AddTypeClient(TypeClientViewModel newTypeClientViewModel)
        {
            var typeClient = new TypeClient
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newTypeClientViewModel.iPAddress,

                Name = newTypeClientViewModel.name,
                Description = newTypeClientViewModel.description
            };

            _typeClientRepository.Add(typeClient);
            return newTypeClientViewModel;
        }

        public bool UpdateTypeClient(TypeClientViewModel typeClientViewModel)
        {
            if (typeClientViewModel.id == null)
                return false;

            var typeClient = _typeClientRepository.GetById((int)typeClientViewModel.id);

            if (typeClient == null) return false;

            typeClient.ModifiedDate = DateTime.Now;
            typeClient.IPAddress = typeClientViewModel.iPAddress;

            typeClient.Name = typeClientViewModel.name;
            typeClient.Description = typeClientViewModel.description;

            return _typeClientRepository.Update(typeClient);
        }

        public bool DeleteTypeClient(int id)
        {
            return _typeClientRepository.Delete(id);
        }
    }
}
