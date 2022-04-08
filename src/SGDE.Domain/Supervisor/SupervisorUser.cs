// ReSharper disable InconsistentNaming
namespace SGDE.Domain.Supervisor
{
    #region Using

    using Converters;
    using Domain.Helpers;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public async Task<UserViewModel> Authenticate(string username, string password, CancellationToken ct = default(CancellationToken))
        {
            var userViewModel = UserConverter.Convert(await _userRepository.Authenticate(username, password, ct));
            if (userViewModel == null)
                return null;

            return userViewModel;
        }

        public async Task<List<UserViewModel>> GetAllUserAsync(CancellationToken ct = default(CancellationToken))
        {
            return UserConverter.ConvertList(await _userRepository.GetAllAsync(ct));
        }

        public async Task<UserViewModel> GetUserByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            var userViewModel = UserConverter.Convert(await _userRepository.GetByIdAsync(id, ct));
            return userViewModel;
        }

        public async Task<UserViewModel> AddUserAsync(UserViewModel newUserViewModel, CancellationToken ct = default(CancellationToken))
        {
            var user = new User
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newUserViewModel.iPAddress,

                Name = newUserViewModel.name,
                Surname = newUserViewModel.surname,
                Username = newUserViewModel.username,
                BirthDate = string.IsNullOrEmpty(newUserViewModel.birthDate)
                    ? null
                    : (DateTime?)DateTime.Parse(newUserViewModel.birthDate),
                Email = newUserViewModel.email,
                Password = "123456"
            };

            await _userRepository.AddAsync(user, ct);

            return newUserViewModel;
        }

        public async Task<bool> UpdateUserAsync(UserViewModel userViewModel, CancellationToken ct = default(CancellationToken))
        {
            if (userViewModel.id == null)
                return false;

            var user = await _userRepository.GetByIdAsync((int)userViewModel.id, ct);

            if (user == null) return false;

            user.ModifiedDate = DateTime.Now;
            user.IPAddress = userViewModel.iPAddress;

            user.Name = userViewModel.name;
            user.Surname = userViewModel.surname;
            user.Username = userViewModel.username;
            user.BirthDate = string.IsNullOrEmpty(userViewModel.birthDate)
                ? null
                : (DateTime?)DateTime.Parse(userViewModel.birthDate);
            user.Email = userViewModel.email;

            return await _userRepository.UpdateAsync(user, ct);
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return await _userRepository.DeleteAsync(id, ct);
        }

        public QueryResult<UserViewModel> GetAllUsers(
            int skip = 0,
            int take = 0,
            string orderBy = null,
            string filter = null,
            List<int> roles = null,
            bool showAllEmployees = true)
        {
            var queryResult = _userRepository.GetAll(skip, take, orderBy, filter, roles, showAllEmployees);
            var data = UserConverter.ConvertList(queryResult.Data);
            foreach (var item in data)
            {
                UpdateStateUser(item);
            }

            return new QueryResult<UserViewModel>
            {
                Data = data,
                Count = queryResult.Count
            };
        }

        public UserViewModel GetUserById(int id)
        {
            var userViewModel = UserConverter.Convert(_userRepository.GetById(id));
            UpdateStateUser(userViewModel);

            return userViewModel;
        }

        public UserViewModel AddUser(UserViewModel newUserViewModel)
        {
            if (newUserViewModel.roleId == 3 && newUserViewModel.userProfessions == null)
                throw new Exception("Debes seleccionar al menos una profesión para este trabajador");

            var user = new User
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newUserViewModel.iPAddress,

                Name = newUserViewModel.name,
                Surname = newUserViewModel.surname,
                Username = newUserViewModel.username,
                Dni = newUserViewModel.dni,
                SecuritySocialNumber = newUserViewModel.securitySocialNumber,
                BirthDate = string.IsNullOrEmpty(newUserViewModel.birthDate)
                    ? null
                    : (DateTime?)DateTime.Parse(newUserViewModel.birthDate),
                Email = newUserViewModel.email,
                Address = newUserViewModel.address,
                PhoneNumber = newUserViewModel.phoneNumber,
                Observations = newUserViewModel.observations,
                AccountNumber = newUserViewModel.accountNumber,
                Photo = newUserViewModel.photo,
                RoleId = newUserViewModel.roleId,
                Password = "123456"
            };

            if (newUserViewModel.roleId == 3)
            {
                var result = _userRepository.AddWithProfessions(user, newUserViewModel.userProfessions);
                newUserViewModel.id = result.Id;
            }
            else
            {
                var result = _userRepository.Add(user);
                newUserViewModel.id = result.Id;
            }

            return newUserViewModel;
        }

        public bool UpdateUser(UserViewModel userViewModel)
        {
            if (userViewModel.id == null)
                return false;

            if (userViewModel.roleId == 3 && userViewModel.userProfessions == null)
                throw new Exception("Debes seleccionar al menos una profesión para este trabajador");

            var user = _userRepository.GetById((int)userViewModel.id);

            if (user == null) return false;

            user.ModifiedDate = DateTime.Now;
            user.IPAddress = userViewModel.iPAddress;

            user.Name = userViewModel.name;
            user.Surname = userViewModel.surname;
            user.Username = userViewModel.username;
            user.Dni = userViewModel.dni;
            user.SecuritySocialNumber = userViewModel.securitySocialNumber;
            //user.BirthDate = string.IsNullOrEmpty(userViewModel.birthDate)
            //    ? null
            //    : (DateTime?)DateTime.ParseExact(userViewModel.birthDate, "yyyy/MM/dd", null);
            user.BirthDate = string.IsNullOrEmpty(userViewModel.birthDate)
                ? null
                : (DateTime?)DateTime.Parse(userViewModel.birthDate);
            user.Email = userViewModel.email;
            user.Address = userViewModel.address;
            user.PhoneNumber = userViewModel.phoneNumber;
            user.Observations = userViewModel.observations;
            user.AccountNumber = userViewModel.accountNumber;
            user.Photo = userViewModel.photo;
            user.RoleId = userViewModel.roleId;
            user.WorkId = userViewModel.workId;
            user.ClientId = userViewModel.clientId;

            var toAdd = userViewModel.userProfessions
                .Except(user.UserProfessions.Select(x => x.ProfessionId)).ToList();

            foreach (var profession in toAdd)
            {
                _userProfessionRepository.Add(new UserProfession
                {
                    UserId = user.Id,
                    ProfessionId = profession
                });
            }

            var toRemove = user.UserProfessions
                .Select(x => x.ProfessionId)
                .Except(userViewModel.userProfessions).ToList();

            foreach (var profession in toRemove)
            {
                _userProfessionRepository.Delete(user.Id, profession);
            }


            return _userRepository.Update(user);
        }

        public bool UpdatePassword(UserViewModel userViewModel)
        {
            if (userViewModel.id == null)
                return false;

            var user = _userRepository.GetById((int)userViewModel.id);

            if (user == null) return false;

            if (user.Password != userViewModel.password)
                throw new Exception("La contraseña introducida no coincide con la guardada");

            if (user.Password == userViewModel.newPassword)
                throw new Exception("La contraseña introducida no puede coincidir con la guardada");

            user.Password = userViewModel.newPassword;

            return _userRepository.Update(user);
        }

        public bool RestorePassword(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null) return false;

            user.Password = "123456";

            return _userRepository.Update(user);
        }

        public bool UpdateUserPhoto(int userId, byte[] photo)
        {
            var user = _userRepository.GetById(userId);

            if (user == null) return false;

            user.ModifiedDate = DateTime.Now;
            user.Photo = photo;

            return _userRepository.Update(user);
        }

        public bool DeleteUser(int id)
        {
            return _userRepository.Delete(id);
        }

        private void UpdateStateUser(UserViewModel userViewModel)
        {
            var initText = "A este usuario le faltan los siguientes datos/documentos: ";
            userViewModel.stateDescription = initText;

            if (string.IsNullOrEmpty(userViewModel.name))
                userViewModel.stateDescription += "nombre, ";
            if (string.IsNullOrEmpty(userViewModel.surname))
                userViewModel.stateDescription += "apellidos, ";
            if (string.IsNullOrEmpty(userViewModel.dni))
                userViewModel.stateDescription += "DNI, ";
            if (string.IsNullOrEmpty(userViewModel.securitySocialNumber))
                userViewModel.stateDescription += "número de la seguridad social, ";
            if (string.IsNullOrEmpty(userViewModel.birthDate))
                userViewModel.stateDescription += "fecha de nacimineto, ";
            if (string.IsNullOrEmpty(userViewModel.address))
                userViewModel.stateDescription += "dirección, ";
            if (string.IsNullOrEmpty(userViewModel.phoneNumber))
                userViewModel.stateDescription += "teléfono de contacto, ";
            if (string.IsNullOrEmpty(userViewModel.accountNumber))
                userViewModel.stateDescription += "número de cuenta, ";

            var documentsPendings = _userRepository.GetPendingDocuments((int)userViewModel.id);
            userViewModel.stateDescription += string.Join(", ", documentsPendings.Select(x => x.Name));

            if (userViewModel.stateDescription.EndsWith(", "))
                userViewModel.stateDescription = userViewModel.stateDescription.Substring(0, userViewModel.stateDescription.Length - 2);

            if (userViewModel.stateDescription == initText)
            {
                userViewModel.stateDescription = "Trabajador con documentación correcta";
                userViewModel.state = 0;
            }
            else
            {
                userViewModel.state = 1;
            }
        }
    }
}
