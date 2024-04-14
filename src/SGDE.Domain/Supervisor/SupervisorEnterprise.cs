using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public List<EnterpriseViewModel> GetAllEnterprise()
        {
            return EnterpriseConverter.ConvertList(_enterpriseRepository.GetAll());
        }

        public List<EnterpriseViewModel> GetEnterpriseByUser(int userId)
        {
            return EnterpriseConverter.ConvertList(_enterpriseRepository.GetByUserId(userId));
        }

        public EnterpriseViewModel GetEnterpriseById(int id)
        {
            var enterpriseViewModel = EnterpriseConverter.Convert(_enterpriseRepository.GetById(id));

            return enterpriseViewModel;
        }

        public EnterpriseViewModel AddEnterprise(EnterpriseViewModel newEnterpriseViewModel)
        {
            var enterprise = new Enterprise
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newEnterpriseViewModel.iPAddress,

                Name = newEnterpriseViewModel.name,
                Alias = newEnterpriseViewModel.alias,
                CIF = newEnterpriseViewModel.cif,
                Address = newEnterpriseViewModel.address,
                PhoneNumber = newEnterpriseViewModel.phoneNumber
            };

            _enterpriseRepository.Add(enterprise);
            return newEnterpriseViewModel;
        }

        public bool UpdateEnterprise(EnterpriseViewModel enterpriseViewModel)
        {
            if (enterpriseViewModel.id == null)
                return false;

            var enterprise = _enterpriseRepository.GetById((int)enterpriseViewModel.id);

            if (enterprise == null) return false;

            enterprise.ModifiedDate = DateTime.Now;
            enterprise.IPAddress = enterpriseViewModel.iPAddress;

            enterprise.Name = enterpriseViewModel.name;
            enterprise.Alias = enterpriseViewModel.alias;
            enterprise.CIF = enterpriseViewModel.cif;
            enterprise.Address = enterpriseViewModel.address;
            enterprise.PhoneNumber = enterpriseViewModel.phoneNumber;

            return _enterpriseRepository.Update(enterprise);
        }

        public bool DeleteEnterprise(int id)
        {
            return _enterpriseRepository.Delete(id);
        }
    }
}
