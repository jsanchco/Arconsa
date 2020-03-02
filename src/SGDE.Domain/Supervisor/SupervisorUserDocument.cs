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
        public List<UserDocumentViewModel> GetAllUserDocument()
        {
            return UserDocumentConverter.ConvertList(_userDocumentRepository.GetAll());
        }

        public UserDocumentViewModel GetUserDocumentById(int id)
        {
            var userDocumentViewModel = UserDocumentConverter.Convert(_userDocumentRepository.GetById(id));

            return userDocumentViewModel;
        }

        public UserDocumentViewModel AddUserDocument(UserDocumentViewModel newUserDocumentViewModel)
        {
            var userDocument = new UserDocument
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newUserDocumentViewModel.iPAddress,

                Description = newUserDocumentViewModel.description,
                Observations = newUserDocumentViewModel.observations,
                File = newUserDocumentViewModel.file,
                TypeDocumentId = newUserDocumentViewModel.typeDocumentId,
                UserId = newUserDocumentViewModel.userId
            };

            _userDocumentRepository.Add(userDocument);
            return newUserDocumentViewModel;
        }

        public bool UpdateUserDocument(UserDocumentViewModel userDocumentViewModel)
        {
            if (userDocumentViewModel.id == null)
                return false;

            var userDocument = _userDocumentRepository.GetById((int)userDocumentViewModel.id);

            if (userDocument == null) return false;

            userDocument.ModifiedDate = DateTime.Now;
            userDocument.IPAddress = userDocumentViewModel.iPAddress;

            userDocument.Description = userDocumentViewModel.description;
            userDocument.Observations = userDocumentViewModel.observations;
            userDocument.File = userDocumentViewModel.file;
            userDocument.TypeDocumentId = userDocumentViewModel.typeDocumentId;
            userDocument.UserId = userDocumentViewModel.userId;

            return _userDocumentRepository.Update(userDocument);
        }

        public bool DeleteUserDocument(int id)
        {
            return _userDocumentRepository.Delete(id);
        }
    }
}
