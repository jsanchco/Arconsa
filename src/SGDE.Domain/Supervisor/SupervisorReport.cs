namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            return ReportResultConverter.ConvertList(_dailySigningRepository.GetByWorkId(reportViewModel.startDate, reportViewModel.endDate, (int)reportViewModel.workId));
        }

        public List<ReportResultViewModel> GetHoursByClient(ReportQueryViewModel reportViewModel)
        {
            return ReportResultConverter.ConvertList(_dailySigningRepository.GetByClientId(reportViewModel.startDate, reportViewModel.endDate, (int)reportViewModel.clientId));
        }

        public List<ReportVariousInfoViewModel> GetHoursByAllUser(ReportQueryAllViewModel reportAllViewModel)
        {
            var users = _userRepository.GetAll(0, 0, null, null, new List<int> { 3 });
            var result = new List<ReportVariousInfoViewModel>();
            foreach(var user in users.Data)
            {
                var listReportResultViewModel = ReportResultConverter.ConvertList(_dailySigningRepository.GetByUserId(reportAllViewModel.startDate, reportAllViewModel.endDate, user.Id));
                result.Add(new ReportVariousInfoViewModel
                {
                    workerName = $"{user.Name} {user.Surname}",
                    totalHoursOrdinary = listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.hours).Sum(),
                    totalHoursExtraordinary = listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.hours).Sum(),
                    totalHoursFestive = listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.hours).Sum(),
                    priceTotalHoursOrdinary = (double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHour).Sum(),
                    priceTotalHoursSaleOrdinary = (double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHourSale).Sum(),
                    priceTotalHoursExtraordinary = (double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHour).Sum(),
                    priceTotalHoursSaleExtraordinary = (double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHourSale).Sum(),
                    priceTotalHoursFestive = (double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHour).Sum(),
                    priceTotalHoursSaleFestive = (double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHourSale).Sum(),
                });
            }
            return result;
        }

        public List<ReportVariousInfoViewModel> GetHoursByAllWork(ReportQueryAllViewModel reportAllViewModel)
        {
            var users = _userRepository.GetAll(0, 0, null, null, new List<int> { 3 });
            foreach (var user in users.Data)
            {
                var dailySignings = _dailySigningRepository.GetByUserId(reportAllViewModel.startDate, reportAllViewModel.endDate, user.Id);
            }
            return null;
        }

        public List<ReportVariousInfoViewModel> GetHoursByAllClient(ReportQueryAllViewModel reportAllViewModel)
        {
            var users = _userRepository.GetAll(0, 0, null, null, new List<int> { 3 });
            foreach (var user in users.Data)
            {
                var dailySignings = _dailySigningRepository.GetByUserId(reportAllViewModel.startDate, reportAllViewModel.endDate, user.Id);
            }
            return null;
        }
    }
}
