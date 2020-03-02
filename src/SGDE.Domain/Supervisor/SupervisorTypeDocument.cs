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
        public List<TypeDocumentViewModel> GetAllTypeDocument()
        {
            return TypeDocumentConverter.ConvertList(_typeDocumentRepository.GetAll());
        }

        public TypeDocumentViewModel GetTypeDocumentById(int id)
        {
            var typeDocumentViewModel = TypeDocumentConverter.Convert(_typeDocumentRepository.GetById(id));

            return typeDocumentViewModel;
        }

        public TypeDocumentViewModel AddTypeDocument(TypeDocumentViewModel newTypeDocumentViewModel)
        {
            var typeDocument = new TypeDocument
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newTypeDocumentViewModel.iPAddress,

                Name = newTypeDocumentViewModel.name,
                Description = newTypeDocumentViewModel.description
            };

            _typeDocumentRepository.Add(typeDocument);
            return newTypeDocumentViewModel;
        }

        public bool UpdateTypeDocument(TypeDocumentViewModel typeDocumentViewModel)
        {
            if (typeDocumentViewModel.id == null)
                return false;

            var typeDocument = _typeDocumentRepository.GetById((int)typeDocumentViewModel.id);

            if (typeDocument == null) return false;

            typeDocument.ModifiedDate = DateTime.Now;
            typeDocument.IPAddress = typeDocumentViewModel.iPAddress;

            typeDocument.Name = typeDocumentViewModel.name;
            typeDocument.Description = typeDocument.Description;

            return _typeDocumentRepository.Update(typeDocument);
        }

        public bool DeleteTypeDocument(int id)
        {
            return _typeDocumentRepository.Delete(id);
        }
    }
}
