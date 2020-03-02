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
        public List<RoleViewModel> GetAllRole()
        {
            return RoleConverter.ConvertList(_roleRepository.GetAll());
        }

        public RoleViewModel GetRoleById(int id)
        {
            var roleViewModel = RoleConverter.Convert(_roleRepository.GetById(id));

            return roleViewModel;
        }

        public RoleViewModel AddRole(RoleViewModel newRoleViewModel)
        {
            var role = new Role
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newRoleViewModel.iPAddress,

                Name = newRoleViewModel.name
            };

            _roleRepository.Add(role);
            return newRoleViewModel;
        }

        public bool UpdateRole(RoleViewModel roleViewModel)
        {
            if (roleViewModel.id == null)
                return false;

            var role = _roleRepository.GetById((int)roleViewModel.id);

            if (role == null) return false;

            role.ModifiedDate = DateTime.Now;
            role.IPAddress = roleViewModel.iPAddress;

            role.Name = roleViewModel.name;

            return _roleRepository.Update(role);
        }

        public bool DeleteRole(int id)
        {
            return _roleRepository.Delete(id);
        }
    }
}
