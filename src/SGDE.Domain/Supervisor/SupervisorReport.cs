﻿namespace SGDE.Domain.Supervisor
{
    #region Using

    using Converters;
    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var users = _userRepository.GetAll(0, 0, null, reportAllViewModel.enterpriseId, null, new List<int> { 3 });
            var result = new List<ReportVariousInfoViewModel>();
            foreach (var user in users.Data)
            {
                var listReportResultViewModel = ReportResultConverter.ConvertList(_dailySigningRepository.GetByUserId(reportAllViewModel.startDate, reportAllViewModel.endDate, user.Id));
                result.Add(new ReportVariousInfoViewModel
                {
                    workerName = $"{user.Name} {user.Surname}",
                    totalHoursOrdinary = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursExtraordinary = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursFestive = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursNocturnal = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.hours).Sum(), 3),
                    priceTotalHoursOrdinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleOrdinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursExtraordinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleExtraordinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursFestive = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleFestive = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursNocturnal = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleNocturnal = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceDiary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 5)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceSaleDiary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 5)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    totalEmbargos = user.Embargos.Where(x => !x.Paid)
                                                .Sum(y =>
                                                    y.DetailEmbargos
                                                        .Where(w => w.DatePay <= reportAllViewModel.endDate && w.DatePay >= reportAllViewModel.startDate).Sum(x => x.Amount)),
                    hasEmbargosPendings = user.Embargos.Any(x => x.Paid == false),
                    totalAdvances = user.Advances.Where(x => !x.Paid)
                                                .Sum(y => y.Amount),
                    hasAdvancesPendings = user.Advances.Any(x => x.Paid == false)
                });
            }

            if (!reportAllViewModel.showCeros)
            {
                result = result.Where(x =>
                    x.totalHoursOrdinary != 0 ||
                    x.totalHoursExtraordinary != 0 ||
                    x.totalHoursFestive != 0 ||
                    x.totalHoursNocturnal != 0 ||
                    x.priceTotalHoursOrdinary != 0 ||
                    x.priceTotalHoursSaleOrdinary != 0 ||
                    x.priceTotalHoursExtraordinary != 0 ||
                    x.priceTotalHoursSaleExtraordinary != 0 ||
                    x.priceTotalHoursFestive != 0 ||
                    x.priceTotalHoursSaleFestive != 0 ||
                    x.priceTotalHoursNocturnal != 0 ||
                    x.priceTotalHoursSaleNocturnal != 0 ||
                    x.priceDiary != 0 ||
                    x.priceSaleDiary != 0)
                    .ToList();
            }

            return result;
        }

        public List<ReportVariousInfoViewModel> GetHoursByAllWork(ReportQueryAllViewModel reportAllViewModel)
        {
            var works = _workRepository.GetAll(0, 0, reportAllViewModel.enterpriseId, null, 0);
            var result = new List<ReportVariousInfoViewModel>();
            foreach (var work in works.Data)
            {
                var listReportResultViewModel = ReportResultConverter.ConvertList(_dailySigningRepository.GetByWorkId(reportAllViewModel.startDate, reportAllViewModel.endDate, work.Id));
                result.Add(new ReportVariousInfoViewModel
                {
                    workName = work.Name,
                    totalWorkers = listReportResultViewModel.Select(x => x.userName).Distinct().Count(),
                    totalHoursOrdinary = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursExtraordinary = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursFestive = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursNocturnal = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.hours).Sum(), 3),
                    priceTotalHoursOrdinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleOrdinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursExtraordinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleExtraordinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursFestive = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleFestive = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursNocturnal = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleNocturnal = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceDiary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 5)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceSaleDiary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 5)
                                                .Select(x => x.priceHourSale).Sum(), 3)
                });
            }

            if (!reportAllViewModel.showCeros)
            {
                result = result.Where(x =>
                    x.totalHoursOrdinary != 0 ||
                    x.totalHoursExtraordinary != 0 ||
                    x.totalHoursFestive != 0 ||
                    x.totalHoursNocturnal != 0 ||
                    x.priceTotalHoursOrdinary != 0 ||
                    x.priceTotalHoursSaleOrdinary != 0 ||
                    x.priceTotalHoursExtraordinary != 0 ||
                    x.priceTotalHoursSaleExtraordinary != 0 ||
                    x.priceTotalHoursFestive != 0 ||
                    x.priceTotalHoursSaleFestive != 0 ||
                    x.priceTotalHoursNocturnal != 0 ||
                    x.priceTotalHoursSaleNocturnal != 0 ||
                    x.priceDiary != 0 ||
                    x.priceSaleDiary != 0)
                    .ToList();
            }

            return result;
        }

        public List<ReportVariousInfoViewModel> GetHoursByAllClient(ReportQueryAllViewModel reportAllViewModel)
        {
            var clients = _clientRepository.GetAll(0, 0, reportAllViewModel.enterpriseId, true, null);
            var result = new List<ReportVariousInfoViewModel>();
            foreach (var client in clients.Data)
            {
                var listReportResultViewModel = ReportResultConverter.ConvertList(_dailySigningRepository.GetByClientId(reportAllViewModel.startDate, reportAllViewModel.endDate, client.Id));
                result.Add(new ReportVariousInfoViewModel
                {
                    clientName = client.Name,
                    totalWorkers = listReportResultViewModel.Select(x => x.userName).Distinct().Count(),
                    totalHoursOrdinary = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursExtraordinary = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursFestive = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.hours).Sum(), 3),
                    totalHoursNocturnal = Math.Round(listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.hours).Sum(), 3),
                    priceTotalHoursOrdinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleOrdinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 1)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursExtraordinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleExtraordinary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 2)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursFestive = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleFestive = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 3)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceTotalHoursNocturnal = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceTotalHoursSaleNocturnal = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 4)
                                                .Select(x => x.priceHourSale).Sum(), 3),
                    priceDiary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 5)
                                                .Select(x => x.priceHour).Sum(), 3),
                    priceSaleDiary = Math.Round((double)listReportResultViewModel
                                                .Where(x => x.hourTypeId == 5)
                                                .Select(x => x.priceHourSale).Sum(), 3)
                });
            }

            if (!reportAllViewModel.showCeros)
            {
                result = result.Where(x =>
                    x.totalHoursOrdinary != 0 ||
                    x.totalHoursExtraordinary != 0 ||
                    x.totalHoursFestive != 0 ||
                    x.totalHoursNocturnal != 0 ||
                    x.priceTotalHoursOrdinary != 0 ||
                    x.priceTotalHoursSaleOrdinary != 0 ||
                    x.priceTotalHoursExtraordinary != 0 ||
                    x.priceTotalHoursSaleExtraordinary != 0 ||
                    x.priceTotalHoursFestive != 0 ||
                    x.priceTotalHoursSaleFestive != 0 ||
                    x.priceTotalHoursNocturnal != 0 ||
                    x.priceTotalHoursSaleNocturnal != 0 ||
                    x.priceDiary != 0 ||
                    x.priceSaleDiary != 0)
                    .ToList();
            }

            return result;
        }

        public List<InvoiceViewModel> GetAllInvoice(ReportQueryAllViewModel reportAllViewModel)
        {
            List<Invoice> invoices = !string.IsNullOrEmpty(reportAllViewModel.filter)
                ? _invoiceRepository.GetAll(0, 0, reportAllViewModel.enterpriseId, reportAllViewModel.filter).Data
                : _invoiceRepository.GetAll(0, 0, reportAllViewModel.enterpriseId).Data;
            var result = InvoiceConverter.ConvertList(invoices.Where(x => x.IssueDate >= reportAllViewModel.startDate &&
                                                                          x.IssueDate <= reportAllViewModel.endDate)
                                                              .ToList());

            return result;
        }

        public List<ReportResultsByWorkViewModel> GetAllResultsByWork(ReportQueryAllViewModel reportAllViewModel)
        {
            return null;
        }

        public List<TracingViewModel> GetTracing(ReportQueryAllViewModel reportAllViewModel)
        {
            var result = _workBudgetRepository.GetByDates(
                reportAllViewModel.enterpriseId,
                reportAllViewModel.startDate,
                reportAllViewModel.endDate,
                reportAllViewModel.filter);

            if (result != null)
            {
                return result.Select(x => new TracingViewModel
                {
                    workBudgetId = x.Id,
                    workBudgetName = x.Name,
                    workId = x.WorkId,
                    workName = x.Work?.Name,
                    workStatus = x.Work?.WorkStatusHistories.OrderByDescending(y => y.DateChange).FirstOrDefault().Value,
                    clientId = x.Work?.ClientId,
                    clientName = x.Work?.Client?.Name,
                    clientEmail = x.Work?.Client?.Email,
                    dateAcceptanceWorkBudget = x.Date,
                    dateOpenWork = x.Work?.OpenDate,
                    dateCloseWork = x.Work?.CloseDate,
                    dateSendWorkBudget = x.Date,
                    workBudgetTotalContract = x.TotalContract,
                    invoiceSum = x.Invoices?.Where(y => y.IsPaid).Sum(y => y.Total),
                    invoiceTotalPaymentSum = x.Invoices?.Sum(y => y.TotalPayment),
                    datesSendInvoices = string.Join(",", x.Invoices?.Where(y => y.IsPaid).Select(y => y.PayDate?.ToString("dd/MM/yyyy")))
                }).ToList();
            }

            return null;
        }

        public QueryResult<WorkClosePageViewModel> GetAllCurrentStatus(int enterpriseId = 0, int skip = 0, int take = 0, string filter = null)
        {
            var worksWithFilter = _workRepository.GetAllLiteIncludeClient(enterpriseId, filter);
            var worksWithSkipAndTake = worksWithFilter.Skip(skip).Take(take);
            var data = new List<WorkClosePageViewModel>();
            foreach (var work in worksWithSkipAndTake)
            {
                var workClosePageViewModel = GetWorkClosePage(work.Id);
                data.Add(workClosePageViewModel);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                data = data
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.clientName?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.workName.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.workBudgetsName?.ToLower()).Contains(filter))
                    .ToList();
            }

            var count = worksWithFilter.Count();
            return (skip != 0 || take != 0)
                ? new QueryResult<WorkClosePageViewModel>
                {
                    Data = data.ToList(),
                    Count = count
                }
                : new QueryResult<WorkClosePageViewModel>
                {
                    Data = data.ToList(),
                    Count = count
                };
        }
    }
}
