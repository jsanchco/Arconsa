using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public static class CompanyDataConverter
    {
        public static CompanyDataViewModel Convert(CompanyData companyData)
        {
            if (companyData == null)
                return null;

            var companyDataViewModel = new CompanyDataViewModel
            {
                id = companyData.Id,
                addedDate = companyData.AddedDate,
                modifiedDate = companyData.ModifiedDate,
                iPAddress = companyData.IPAddress,

                enterpriseId = companyData.EnterpriseId,
                reference = companyData.Reference,                
                description = companyData.Description,
                observations = companyData.Observations,
                date = companyData.Date,
                dateWarning = companyData.DateWarning,
                dateExpiration = companyData.DateExpiration,
                typeFile = companyData.TypeFile,
                file = companyData.File,
                fileName = companyData.FileName
            };

            return companyDataViewModel;
        }

        public static List<CompanyDataViewModel> ConvertList(IEnumerable<CompanyData> companyDatas)
        {
            return companyDatas?.Select(companyData =>
            {
                var model = new CompanyDataViewModel
                {
                    id = companyData.Id,
                    addedDate = companyData.AddedDate,
                    modifiedDate = companyData.ModifiedDate,
                    iPAddress = companyData.IPAddress,

                    enterpriseId = companyData.EnterpriseId,
                    reference = companyData.Reference,
                    description = companyData.Description,
                    observations = companyData.Observations,
                    date = companyData.Date,
                    dateWarning = companyData.DateWarning,
                    dateExpiration = companyData.DateExpiration,
                    typeFile = companyData.TypeFile,
                    file = companyData.File,
                    fileName = companyData.FileName
                };
                return model;
            })
                .ToList();
        }
    }
}
