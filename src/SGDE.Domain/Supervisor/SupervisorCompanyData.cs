using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<CompanyDataViewModel> GetAllCompanyData(int skip = 0, int take = 0, string filter = null)
        {
            var queryResult = _companyDataRepository.GetAll(skip, take, filter);
            return new QueryResult<CompanyDataViewModel>
            {
                Data = CompanyDataConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public CompanyDataViewModel GetCompanyDataById(int id)
        {
            var companyDataViewModel = CompanyDataConverter.Convert(_companyDataRepository.GetById(id));

            return companyDataViewModel;
        }

        public CompanyDataViewModel AddCompanyData(CompanyDataViewModel newCompanyDataViewModel)
        {
            CheckDates(newCompanyDataViewModel);

            var companyData = new CompanyData
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newCompanyDataViewModel.iPAddress,

                Reference = newCompanyDataViewModel.reference,
                Description = newCompanyDataViewModel.description,
                Observations = newCompanyDataViewModel.observations,
                Date = newCompanyDataViewModel.date,
                DateWarning = newCompanyDataViewModel.dateWarning,
                DateExpiration = newCompanyDataViewModel.dateExpiration,
                File = newCompanyDataViewModel.file,
                TypeFile = newCompanyDataViewModel.typeFile,
                FileName = newCompanyDataViewModel.fileName
            };

            _companyDataRepository.Add(companyData);
            return newCompanyDataViewModel;
        }

        public bool UpdateCompanyData(CompanyDataViewModel companyDataViewModel)
        {
            if (companyDataViewModel.id == null)
                return false;

            CheckDates(companyDataViewModel);

            var companyData = _companyDataRepository.GetById((int)companyDataViewModel.id);

            if (companyData == null) return false;

            companyData.ModifiedDate = DateTime.Now;
            companyData.IPAddress = companyDataViewModel.iPAddress;

            companyData.Reference = companyDataViewModel.reference;
            companyData.Description = companyDataViewModel.description;
            companyData.Observations = companyDataViewModel.observations;
            companyData.Date = companyDataViewModel.date;
            companyData.DateWarning = companyDataViewModel.dateWarning;
            companyData.DateExpiration = companyDataViewModel.dateExpiration;
            companyData.File = companyDataViewModel.file;
            companyData.TypeFile = companyDataViewModel.typeFile;
            companyData.FileName = companyDataViewModel.fileName;

            return _companyDataRepository.Update(companyData);
        }

        public bool DeleteCompanyData(int id)
        {
            return _companyDataRepository.Delete(id);
        }

        private void CheckDates(CompanyDataViewModel newCompanyDataViewModel)
        {
            if (newCompanyDataViewModel.dateExpiration.HasValue && 
                newCompanyDataViewModel.dateExpiration.Value < newCompanyDataViewModel.date)
            {
                throw new Exception("Fechas mal configuradas");
            }

            if (newCompanyDataViewModel.dateWarning.HasValue &&
                newCompanyDataViewModel.dateWarning.Value < newCompanyDataViewModel.date)
            {
                throw new Exception("Fechas mal configuradas");
            }

            if (newCompanyDataViewModel.dateWarning.HasValue &&
                newCompanyDataViewModel.dateExpiration.HasValue &&
                newCompanyDataViewModel.dateWarning.Value > newCompanyDataViewModel.dateExpiration.Value)
            {
                throw new Exception("Fechas mal configuradas");
            }
        }
    }
}
