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
        public List<ReportResultViewModel> GetHoursByUser(ReportQueryViewModel reportViewModel)
        {
            return ReportResultConverter.ConvertList(_dailySigningRepository.GetByUserId(reportViewModel.startDate, reportViewModel.endDate, (int)reportViewModel.workerId));
        }

        public List<ReportResultViewModel> GetHoursByWork(ReportQueryViewModel reportViewModel)
        {
            return ReportResultConverter.ConvertList(_dailySigningRepository.GetByWorkId(reportViewModel.startDate, reportViewModel.endDate, (int)reportViewModel.workerId));
        }

        public List<ReportResultViewModel> GetHoursByClient(ReportQueryViewModel reportViewModel)
        {
            return ReportResultConverter.ConvertList(_dailySigningRepository.GetByClientId(reportViewModel.startDate, reportViewModel.endDate, (int)reportViewModel.workerId));
        }
    }
}
